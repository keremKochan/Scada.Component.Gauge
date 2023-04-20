using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Data.DataList
{
    public static class TScadaDataList
    {
        public static List<TData> DataList = new List<TData>();
        public static Queue<TData> Datas = new Queue<TData>();
        public static ConcurrentDictionary<int, List<TData>> Datalarim = new ConcurrentDictionary<int, List<TData>>();


        public static void AddData(TData Data)
        {
            DataList.Add(Data);
        }

        public static void AddHeatData(TData Data)
        {
            Datas.Enqueue(Data);
        }

        public static TData GetHeatDatas()
        {
            return Datas.Dequeue();
        }

        public static void AddDatam(int DeviceId, TData Data)
        {
            List<TData> datas = new List<TData>() { Data };
            Datalarim.TryAdd(DeviceId, datas);
        }

        public static void GetData(int DeviceId)
        {
            List<TData> datas = new List<TData>();
            Datalarim.TryGetValue(DeviceId, out datas);
        }

    }
}
