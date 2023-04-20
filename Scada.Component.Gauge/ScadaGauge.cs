using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Scada.Data;

namespace Scada.Component.Gauge
{
    public partial class ScadaGauge : UserControl
    {        
        private int Count = 0;
        private int FHeat;
        public int Heat
        {
            get { return GetHeat(); }
            set { SetHeat(value); }
        }
        private void SetHeat(int value)
        {
            FHeat = value;
            label1.Text = FHeat.ToString();
            trackBar1.Value = FHeat;
            if (FHeat >= FMaxHeat)
            {
                if (FAlarmActive)
                    Blink();
            }
        }
        private int GetHeat()
        {
            return FHeat;
        }

        private TDevice FDevice;
        public TDevice Device
        {
            get { return GetDevice(); }
            set { SetDevice(value); }
        }

        private void SetDevice(TDevice value)
        {
            FDevice = value;
            LblName.Text = FDevice.DeviceName;
        }

        private TDevice GetDevice()
        {
            return FDevice;
        }

        private int FMaxHeat;
        public int MaxHeat
        {
            get { return GetMaxHeat(); }
            set { SetMaxHeat(value); }
        }
        private void SetMaxHeat(int value)
        {
            FMaxHeat = value;
        }
        private int GetMaxHeat()
        {
            return FMaxHeat;
        }

        private bool FAlarmActive;
        public bool AlarmActive
        {
            get { return GetAlarmActive(); }
            set { SetAlarmActive(value); }
        }
        private void SetAlarmActive(bool value)
        {
            FAlarmActive = value;
        }
        private bool GetAlarmActive()
        {
            return FAlarmActive;
        }

        public ScadaGauge()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        


        private void Blink()
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    if (LblName.BackColor == Color.CornflowerBlue)
                        LblName.BackColor = Color.Red;
                    else
                        LblName.BackColor = Color.CornflowerBlue;
                    Thread.Sleep(50);
                }
            });
        }
    }
}
