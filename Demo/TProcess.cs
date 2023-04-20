using Scada.Data;
using Scada.Data.DataList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Demo
{
    public delegate void RxOnGenerateDevice(List<TDevice> Devices);
    public delegate void RxOnGetDeviceData(List<TData> DataList);

    public class TProcess
    {
        Thread ThrSimulation;
        private List<TDevice> devices;
        private int DeviceCount;
        public event RxOnGenerateDevice OnGenerateDevice;
        public event RxOnGetDeviceData OnGetDeviceData;
        public void CreateDevice(int Count)
        {
            DeviceCount = Count;
            devices = new List<TDevice>();
            for (int i = 0; i < Count; i++)
            {
                TDevice device = new TDevice();
                device.DevicId = new Random().Next(10, 99);                
                Thread.Sleep(20);
                device.DeviceName = device.DevicId.ToString() + ". Cihaz";
                devices.Add(device);
            }
            if (OnGenerateDevice != null)
                OnGenerateDevice(devices);
        }

        public void StartGenerateData()
        {
            if (ThrSimulation == null)
                ThrSimulation = new Thread(new ThreadStart(DoStartGenerateData));

            ThrSimulation.Start();
        }

        private void DoStartGenerateData()
        {
            foreach (var device in devices)
            {
                TDeviceProcess DeviceProcess = new TDeviceProcess(device);
                DeviceProcess.Start();
            }
        }

        public void GetDeviceDatas()
        {
            foreach (var device in devices)
            {
                TDeviceDataProcess DeviceProcess = new TDeviceDataProcess(device);
                DeviceProcess.OnGetDeviceData += DeviceProcess_OnGetDeviceData;
                DeviceProcess.Start();
            }
        }

        private void DeviceProcess_OnGetDeviceData(List<TData> DataList)
        {
            if (OnGetDeviceData!=null)
                OnGetDeviceData(DataList);
        }
    }


    public class TDeviceProcess
    {
        private TDevice device;
        Thread MyThread;
        public TDeviceProcess(TDevice Device)
        {
            device = Device;
        }

        public void Start()
        {
            if (MyThread == null)
                MyThread = new Thread(new ThreadStart(DoStart));
            MyThread.Start();
        }

        private void DoStart()
        {
            while (true)
            {
                lock (TScadaDataList.DataList)
                {
                    TData data = new TData();
                    data.DataDate = DateTime.Now;
                    data.Heat = new Random().Next(1, 100);
                    Thread.Sleep(20);
                    data.DeviceId = device.DevicId;
                    TScadaDataList.AddData(data);
                }                
                int SleepTime = new Random().Next(3, 9);
                Thread.Sleep(SleepTime * 1000);
            }
        }

    }

    public class TDeviceDataProcess
    {
        public event RxOnGetDeviceData OnGetDeviceData;
        private TDevice device;
        private Thread MyThread;
        public TDeviceDataProcess(TDevice Device)
        {
            device = Device;
        }

        public void Start()
        {
            if (MyThread == null)
                MyThread = new Thread(new ThreadStart(DoStart));
            MyThread.Start();
        }

        private void DoStart()
        {
            while (true)
            {
                List<TData> dataList = new List<TData>();

                lock (TScadaDataList.DataList)
                {
                    dataList = (from deviceData in TScadaDataList.DataList where deviceData.DeviceId == device.DevicId orderby deviceData.DataDate select deviceData).ToList();
                    foreach (var data in dataList)
                    {
                        TScadaDataList.DataList.Remove(data);
                    }
                }
                if (OnGetDeviceData != null)
                    OnGetDeviceData(dataList);
                Thread.Sleep(500);
            }
        }

    }


}
