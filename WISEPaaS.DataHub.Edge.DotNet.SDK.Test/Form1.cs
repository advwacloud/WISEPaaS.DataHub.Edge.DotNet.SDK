using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WISEPaaS.DataHub.Edge.DotNet.SDK;
using WISEPaaS.DataHub.Edge.DotNet.SDK.Model;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Test
{
    public partial class Form1 : Form
    {
        private EdgeAgent _edgeAgent;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load( object sender, EventArgs e )
        {
        }

        private void _edgeAgent_MessageReceived( object sender, MessageReceivedEventArgs e )
        {
            switch ( e.Type )
            {
                case MessageType.WriteValue:
                    WriteValueCommand wvcMsg = ( WriteValueCommand ) e.Message;
                    foreach ( var device in wvcMsg.DeviceList )
                    {
                        Console.WriteLine( "DeviceId: {0}", device.Id );
                        foreach ( var tag in device.TagList )
                        {
                            Console.WriteLine( "TagName: {0}, Value: {1}", tag.Name, tag.Value.ToString() );
                        }
                    }
                    break;
                case MessageType.WriteConfig:
                    Console.WriteLine( "UTC Time: {0}", e.Message.ToString() );
                    break;
                case MessageType.ConfigAck:
                    ConfigAck cfgAckMsg = ( ConfigAck ) e.Message;
                    MessageBox.Show( string.Format( "Upload Config Result: {0}", cfgAckMsg.Result.ToString() ) );
                    break;
            }
        }

        private void _edgeAgent_Disconnected( object sender, DisconnectedEventArgs e )
        {
            if ( this.lblStatus.InvokeRequired )
            {
                BeginInvoke( ( MethodInvoker ) delegate ()
                {
                    lblStatus.Text = "DISCONNECTED";
                    lblStatus.BackColor = Color.Silver;
                } );
            }
        }

        private void _edgeAgent_Connected( object sender, EdgeAgentConnectedEventArgs e )
        {
            if ( this.lblStatus.InvokeRequired )
            {
                BeginInvoke( ( MethodInvoker ) delegate ()
                {
                    lblStatus.Text = "CONNECTED";
                    lblStatus.BackColor = Color.Green;
                } );
            }
        }

        private void btnConnect_Click( object sender, EventArgs e )
        {
            if ( string.IsNullOrEmpty( txtNodeId.Text ) )
            {
                MessageBox.Show( "NodeID can not be null !" );
                return;
            }

            if ( _edgeAgent == null )
            {
                EdgeAgentOptions options = new EdgeAgentOptions()
                {
                    AutoReconnect = true,
                    ReconnectInterval = 1000,
                    NodeId = txtNodeId.Text.Trim(),
                    Heartbeat = Convert.ToInt32( numHeartbeat.Value ) * 1000,   // default is 60 seconds,
                    DataRecover = true,
                    ConnectType = ( string.IsNullOrEmpty( txtDCCSKey.Text ) == false ) ? ConnectType.DCCS : ConnectType.MQTT,
                    UseSecure = ckbSecure.Checked
                };

                switch ( options.ConnectType )
                {
                    case ConnectType.DCCS:
                        options.DCCS = new DCCSOptions()
                        {
                            CredentialKey = txtDCCSKey.Text.Trim(),
                            APIUrl = txtDCCSAPIUrl.Text.Trim()
                        };
                        break;
                    case ConnectType.MQTT:
                        options.MQTT = new MQTTOptions()
                        {
                            HostName = txtHostName.Text.Trim(),
                            Port = Convert.ToInt32( txtPort.Text.Trim() ),
                            Username = txtUserName.Text.Trim(),
                            Password = txtPassword.Text.Trim(),
                            ProtocolType = Protocol.TCP
                        };
                        break;
                }

                _edgeAgent = new EdgeAgent( options );

                _edgeAgent.Connected += _edgeAgent_Connected;
                _edgeAgent.Disconnected += _edgeAgent_Disconnected;
                _edgeAgent.MessageReceived += _edgeAgent_MessageReceived;
            }
            else
            {
                _edgeAgent.Options.NodeId = txtNodeId.Text.Trim();
                _edgeAgent.Options.ConnectType = ( string.IsNullOrEmpty( txtDCCSKey.Text ) == false ) ? ConnectType.DCCS : ConnectType.MQTT;
                _edgeAgent.Options.UseSecure = ckbSecure.Checked;
                switch ( _edgeAgent.Options.ConnectType )
                {
                    case ConnectType.DCCS:
                        _edgeAgent.Options.DCCS = new DCCSOptions()
                        {
                            CredentialKey = txtDCCSKey.Text.Trim(),
                            APIUrl = txtDCCSAPIUrl.Text.Trim()
                        };
                        break;
                    case ConnectType.MQTT:
                        _edgeAgent.Options.MQTT = new MQTTOptions()
                        {
                            HostName = txtHostName.Text.Trim(),
                            Port = Convert.ToInt32( txtPort.Text.Trim() ),
                            Username = txtUserName.Text.Trim(),
                            Password = txtPassword.Text.Trim(),
                            ProtocolType = Protocol.TCP
                        };
                        break;
                }
            }

            _edgeAgent.Connect();
        }

        private void btnDisconnect_Click( object sender, EventArgs e )
        {
            timer1.Enabled = false;

            if ( _edgeAgent == null )
                return;

            _edgeAgent.Disconnect();
        }

        private void btnUploadConfig_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            EdgeConfig config = new EdgeConfig();
            for ( int i = 1; i <= numDeviceCount.Value; i++ )
            {
                EdgeConfig.DeviceConfig device = new EdgeConfig.DeviceConfig()
                {
                    Id = "Device" + i,
                    Name = "Device " + i,
                    Type = "Smart Device",
                    Description = "Device " + i,
                    RetentionPolicyName = txtRPName.Text
                };

                for ( int j = 1; j <= numATagCount.Value; j++ )
                {
                    EdgeConfig.AnalogTagConfig analogTag = new EdgeConfig.AnalogTagConfig()
                    {
                        Name = "ATag" + j,
                        Description = "ATag " + j,
                        ReadOnly = false,
                        ArraySize = 0,
                        SpanHigh = 1000,
                        SpanLow = 0,
                        EngineerUnit = string.Empty,
                        IntegerDisplayFormat = 4,
                        FractionDisplayFormat = 2,
                        SendWhenValueChanged = false
                    };
                    device.AnalogTagList.Add( analogTag );
                }
                for ( int j = 1; j <= numDTagCount.Value; j++ )
                {
                    EdgeConfig.DiscreteTagConfig discreteTag = new EdgeConfig.DiscreteTagConfig()
                    {
                        Name = "DTag" + j,
                        Description = "DTag " + j,
                        ReadOnly = false,
                        ArraySize = 0,
                        State0 = "0",
                        State1 = "1",
                        State2 = string.Empty,
                        State3 = string.Empty,
                        State4 = string.Empty,
                        State5 = string.Empty,
                        State6 = string.Empty,
                        State7 = string.Empty,
                        SendWhenValueChanged = true
                    };
                    device.DiscreteTagList.Add( discreteTag );
                }
                for ( int j = 1; j <= numTTagCount.Value; j++ )
                {
                    EdgeConfig.TextTagConfig textTag = new EdgeConfig.TextTagConfig()
                    {
                        Name = "TTag" + j,
                        Description = "TTag " + j,
                        ReadOnly = false,
                        ArraySize = 0,
                        SendWhenValueChanged = true
                    };
                    device.TextTagList.Add( textTag );
                }
                for ( int j = 1; j <= numAArrayTagCount.Value; j++ )
                {
                    EdgeConfig.AnalogTagConfig analogTag = new EdgeConfig.AnalogTagConfig()
                    {
                        Name = "ArrayTag" + j,
                        Description = "ArrayTag " + j,
                        ArraySize = 10,
                        SendWhenValueChanged = true
                    };
                    device.AnalogTagList.Add( analogTag );
                }

                config.Node.DeviceList.Add( device );
            }

            bool result = _edgeAgent.UploadConfig( ActionType.Create, config ).Result;
        }

        private void btnSendData_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            timer1.Interval = ( int ) numDataFreq.Value * 1000;
            timer1.Enabled = !timer1.Enabled;

            if ( timer1.Enabled == true )
            {
                btnSendData.Text = "Stop";
                btnSendData.BackColor = Color.Red;
            }
            else
            {
                btnSendData.Text = "Start to send Data";
                btnSendData.BackColor = Color.Gray;
            }
        }

        private void timer1_Tick( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Reset();
            sw.Start();

            EdgeData data = prepareData();
            bool result = _edgeAgent.SendData( data ).Result;
            sw.Stop();
            Console.WriteLine( sw.Elapsed.TotalMilliseconds.ToString() );

        }


        private EdgeData prepareData()
        {
            Random random = new Random();

            EdgeData data = new EdgeData();
            for ( int i = 1; i <= numDeviceCount.Value; i++ )
            {
                for ( int j = 1; j <= numATagCount.Value; j++ )
                {
                    EdgeData.Tag aTag = new EdgeData.Tag()
                    {
                        DeviceId = "Device" + i,
                        TagName = "ATag" + j,
                        Value = random.Next( 100 )
                    };
                    data.TagList.Add( aTag );
                }
                for ( int j = 1; j <= numDTagCount.Value; j++ )
                {
                    EdgeData.Tag dTag = new EdgeData.Tag()
                    {
                        DeviceId = "Device" + i,
                        TagName = "DTag" + j,
                        Value = DateTime.Now.Second % 2
                    };
                    data.TagList.Add( dTag );
                }
                for ( int j = 1; j <= numTTagCount.Value; j++ )
                {
                    EdgeData.Tag tTag = new EdgeData.Tag()
                    {
                        DeviceId = "Device" + i,
                        TagName = "TTag" + j,
                        Value = "TEST " + j.ToString()
                    };
                    data.TagList.Add( tTag );
                }
                for ( int j = 1; j <= numAArrayTagCount.Value; j++ )
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    for ( int k = 0; k < 10; k++ )
                        dic.Add( k.ToString(), random.Next( 100 ) );
                    EdgeData.Tag arrTag = new EdgeData.Tag()
                    {
                        DeviceId = "Device" + i,
                        TagName = "ArrayTag" + j,
                        Value = dic
                    };
                    data.TagList.Add( arrTag );
                }
            }
            data.Timestamp = DateTime.Now;

            return data;
        }

        bool devStatus = true;
        private void btnDeviceStatus_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            devStatus = !devStatus;
            EdgeDeviceStatus deviceStatus = new EdgeDeviceStatus();
            for ( int i = 1; i <= numDeviceCount.Value; i++ )
            {
                EdgeDeviceStatus.Device device = new EdgeDeviceStatus.Device()
                {
                    Id = "Device" + i,
                    Status = ( Status ) Convert.ToInt32( devStatus )
                };
                deviceStatus.DeviceList.Add( device );
            }
            deviceStatus.Timestamp = DateTime.UtcNow;

            bool result = _edgeAgent.SendDeviceStatus( deviceStatus ).Result;
        }

        private void btnUpdateConfig_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            EdgeConfig config = new EdgeConfig();
            for ( int i = 1; i <= numDeviceCount.Value; i++ )
            {
                EdgeConfig.DeviceConfig device = new EdgeConfig.DeviceConfig()
                {
                    Id = "Device" + i,
                };

                EdgeConfig.AnalogTagConfig analogTag = new EdgeConfig.AnalogTagConfig()
                {
                    Name = "ATag1",
                    ReadOnly = false,
                    SpanHigh = 9999,
                    IntegerDisplayFormat = 5,
                    FractionDisplayFormat = 3
                };
                device.AnalogTagList.Add( analogTag );

                /*for ( int j = 1; j <= numATagCount.Value; j++ )
                {
                    EdgeConfig.AnalogTagConfig analogTag = new EdgeConfig.AnalogTagConfig()
                    {
                        Name = "ATag" + j,
                        Description = "ATag " + j,
                        ReadOnly = false,
                        ArraySize = 0,
                        SpanHigh = 1000,
                        SpanLow = 0,
                        EngineerUnit = string.Empty,
                        IntegerDisplayFormat = 4,
                        FractionDisplayFormat = 2
                    };
                    device.AnalogTagList.Add( analogTag );
                }
                for ( int j = 1; j <= numDTagCount.Value; j++ )
                {
                    EdgeConfig.DiscreteTagConfig discreteTag = new EdgeConfig.DiscreteTagConfig()
                    {
                        Name = "DTag" + j,
                        Description = "DTag " + j,
                        ReadOnly = false,
                        ArraySize = 0,
                        State0 = "0",
                        State1 = "1",
                        State2 = string.Empty,
                        State3 = string.Empty,
                        State4 = string.Empty,
                        State5 = string.Empty,
                        State6 = string.Empty,
                        State7 = string.Empty
                    };
                    device.DiscreteTagList.Add( discreteTag );
                }
                for ( int j = 1; j <= numTTagCount.Value; j++ )
                {
                    EdgeConfig.TextTagConfig textTag = new EdgeConfig.TextTagConfig()
                    {
                        Name = "TTag" + j,
                        Description = "TTag " + j,
                        ReadOnly = false,
                        ArraySize = 0
                    };
                    device.TextTagList.Add( textTag );
                }*/

                config.Node.DeviceList.Add( device );
            }

            bool result = _edgeAgent.UploadConfig( ActionType.Update, config ).Result;
        }

        private void btnDeleteAllConfig_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            EdgeConfig config = new EdgeConfig();
            bool result = _edgeAgent.UploadConfig( ActionType.Delete, config ).Result;
        }

        private void btnDeleteDevices_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            EdgeConfig config = new EdgeConfig();
            for ( int i = 1; i <= numDeviceCount.Value; i++ )
            {
                EdgeConfig.DeviceConfig device = new EdgeConfig.DeviceConfig()
                {
                    Id = "Device" + i
                };

                config.Node.DeviceList.Add( device );
            }

            bool result = _edgeAgent.UploadConfig( ActionType.Delete, config ).Result;
        }

        private void btnDeleteTags_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            EdgeConfig config = new EdgeConfig();
            for ( int i = 1; i <= numDeviceCount.Value; i++ )
            {
                EdgeConfig.DeviceConfig device = new EdgeConfig.DeviceConfig()
                {
                    Id = "Device" + i
                };

                for ( int j = 1; j <= numATagCount.Value; j++ )
                {
                    EdgeConfig.AnalogTagConfig analogTag = new EdgeConfig.AnalogTagConfig()
                    {
                        Name = "ATag" + j
                    };
                    device.AnalogTagList.Add( analogTag );
                }
                for ( int j = 1; j <= numDTagCount.Value; j++ )
                {
                    EdgeConfig.DiscreteTagConfig discreteTag = new EdgeConfig.DiscreteTagConfig()
                    {
                        Name = "DTag" + j
                    };
                    device.DiscreteTagList.Add( discreteTag );
                }
                for ( int j = 1; j <= numTTagCount.Value; j++ )
                {
                    EdgeConfig.TextTagConfig textTag = new EdgeConfig.TextTagConfig()
                    {
                        Name = "TTag" + j
                    };
                    device.TextTagList.Add( textTag );
                }

                config.Node.DeviceList.Add( device );
            }

            bool result = _edgeAgent.UploadConfig( ActionType.Delete, config ).Result;
        }

        private void btnDelsertConfig_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            EdgeConfig config = new EdgeConfig();
            for ( int i = 1; i <= numDeviceCount.Value; i++ )
            {
                EdgeConfig.DeviceConfig device = new EdgeConfig.DeviceConfig()
                {
                    Id = "Device" + i,
                    Name = "Device " + i,
                    Type = "Smart Device",
                    Description = "Device " + i,
                    RetentionPolicyName = txtRPName.Text
                };

                for ( int j = 1; j <= numATagCount.Value; j++ )
                {
                    EdgeConfig.AnalogTagConfig analogTag = new EdgeConfig.AnalogTagConfig()
                    {
                        Name = "ATag" + j,
                        Description = "ATag " + j,
                        ReadOnly = false,
                        ArraySize = 0,
                        SpanHigh = 1000,
                        SpanLow = 0,
                        EngineerUnit = string.Empty,
                        IntegerDisplayFormat = 4,
                        FractionDisplayFormat = 2
                    };
                    device.AnalogTagList.Add( analogTag );
                }
                for ( int j = 1; j <= numDTagCount.Value; j++ )
                {
                    EdgeConfig.DiscreteTagConfig discreteTag = new EdgeConfig.DiscreteTagConfig()
                    {
                        Name = "DTag" + j,
                        Description = "DTag " + j,
                        ReadOnly = false,
                        ArraySize = 0,
                        State0 = "0",
                        State1 = "1",
                        State2 = string.Empty,
                        State3 = string.Empty,
                        State4 = string.Empty,
                        State5 = string.Empty,
                        State6 = string.Empty,
                        State7 = string.Empty
                    };
                    device.DiscreteTagList.Add( discreteTag );
                }
                for ( int j = 1; j <= numTTagCount.Value; j++ )
                {
                    EdgeConfig.TextTagConfig textTag = new EdgeConfig.TextTagConfig()
                    {
                        Name = "TTag" + j,
                        Description = "TTag " + j,
                        ReadOnly = false,
                        ArraySize = 0
                    };
                    device.TextTagList.Add( textTag );
                }
                for ( int j = 1; j <= numAArrayTagCount.Value; j++ )
                {
                    EdgeConfig.AnalogTagConfig arrayTag = new EdgeConfig.AnalogTagConfig()
                    {
                        Name = "ArrayTag" + j,
                        Description = "ArrayTag " + j,
                        ReadOnly = false,
                        ArraySize = 10,
                        SpanHigh = 1000,
                        SpanLow = 0,
                        EngineerUnit = string.Empty,
                        IntegerDisplayFormat = 4,
                        FractionDisplayFormat = 2
                    };
                    device.AnalogTagList.Add( arrayTag );
                }

                config.Node.DeviceList.Add( device );
            }

            bool result = _edgeAgent.UploadConfig( ActionType.Delsert, config ).Result;
        }

        private void btnUpdateData_Click( object sender, EventArgs e )
        {
            if ( _edgeAgent == null )
                return;

            EdgeData data = new EdgeData();
            data.TagList.Add( new EdgeData.Tag()
            {
                DeviceId = "Device1",
                TagName = "ATag1",
                Value = 2
            } );//2020 - 08 - 26T08: 02:37.893Z
            data.Timestamp = new DateTime( 2020, 8, 26, 16, 02, 37, 893 );
            _edgeAgent.UpdateData( data );
        }
    }
}
