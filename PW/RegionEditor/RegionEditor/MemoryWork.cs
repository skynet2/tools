using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RegionEditor
{
    /// <summary>
    /// Класс для работы с памятью 
    /// </summary>
    public class MemoryWork
    {
        //Хэндл открытого процесса
        public IntPtr OpenedProcessHandle { get; set; }
        //ID открытого процесса
        public int OpenedProcessID { get; set; }
        //Память под логин
        public int LoginAllocMemory { get; set; }
        //Память под пароль
        public int PassAllocMemory { get; set; }
        //Память под другие слова
        public int TextAllocMemory { get; set; }
        //Адрес под инжекты
        public int FuncAllocMemory { get; set; }
        //Адрес для пакета
        public int PacketAllocMemory { get; set; }

        /// <summary>
        /// Конструктор класса открывающий память по id процесса извлекаемого из ClientWindow
        /// </summary>
        /// <param name="client_window"></param>
        public MemoryWork(Process client_window)
        {
            //Открываем память выбранного процесса
            OpenProcess(client_window.Id);
            //выделяем память в выбранном процессе
            AllocateMemory();
        }

        /// <summary>
        /// Конструктор класса открывающий память по id процесса
        /// </summary>
        /// <param name="process_id"></param>
        public MemoryWork(int process_id)
        {
            OpenProcess(process_id);
            AllocateMemory();
        }

        /// <summary>
        /// Открывает объект процесса, возвращая дескриптор процесса. 
        /// Так же, дескриптор сохранится в свойстве класса: OpenProcessHandle.
        /// </summary>
        /// <param name="processId">PID процесса, который мы хотитм открыть.</param>
        /// <returns></returns>
        private void OpenProcess(int process_id)
        {
            OpenedProcessHandle = WinApi.OpenProcess(WinApi.ProcessAccessFlags.All, false, process_id);
            OpenedProcessID = process_id;
            return;
        }

        /// <summary>
        /// Закрывает дескриптор открытого процесса.
        /// </summary>
        public void CloseProcess()
        {
            if (OpenedProcessHandle != IntPtr.Zero)
                WinApi.CloseHandle(OpenedProcessHandle);
            OpenedProcessHandle = IntPtr.Zero;
        }

        /// <summary>
        /// Выделяет область памяти в памяти открытого окна  
        /// </summary>
        /// <returns></returns>
        private void AllocateMemory()
        {
            //Выделяем страницу память в 2000 байт
            int alloc_address = WinApi.VirtualAllocEx(this.OpenedProcessHandle, 0, 2500, WinApi.AllocationType.Commit, WinApi.MemoryProtection.ReadWrite);
            //Сначала листа будут прописываться функции
            this.FuncAllocMemory = alloc_address;
            //с отступом в 500 - будет логин
            this.LoginAllocMemory = alloc_address + 500;
            //с отступом в 1000 - будет пароль
            this.PassAllocMemory = alloc_address + 1000;
            //с отступом в 1500 - будет прописываться другой текст
            this.TextAllocMemory = alloc_address + 1500;
            //с отступом в 2000 - будет прописываться пакет
            this.PacketAllocMemory = alloc_address + 2000;
        }
        //Проблема: Под текст память выделяется динамически, а нужно сделать, чтобы память выделялась один раз и потом 
        //только использовалась. 
        //Решение: В выделенной памяти выделить блок: ЛогинМемору - для логина, ПассМемору - для пасса, 
        //OtheTextMemory - память для остальных слов. После использования этого текста он должен затираться. Или даже не так-
        //писать можно поверх старого текста, главное - добавлять конец строки и тогда все будет нормально отрабатывать. 

        public byte ReadByte(Int32 address)
        {
            int read; var buffer = new byte[1];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, buffer.Length, out read);

            return buffer[0];
        }

        /// <summary>
        /// Читает из памяти Int16 по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Int16 ReadInt16(Int32 address)
        {
            int read; var buffer = new byte[2];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, buffer.Length, out read);

            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Читает из памяти Int32 по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Int32 ReadInt32(Int32 address)
        {
            int read; var buffer = new byte[4];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, buffer.Length, out read);

            return BitConverter.ToInt32(buffer, 0);
        }

        public uint ReadUInt32(Int32 address)
        {
            int read; var buffer = new byte[4];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, buffer.Length, out read);

            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Читает из памяти Int64 по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Int64 ReadInt64(Int32 address)
        {
            int read; var buffer = new byte[8];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, buffer.Length, out read);

            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Читает из памяти Float по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Single ReadFloat(Int32 address)
        {
            int read; var buffer = new byte[4];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, buffer.Length, out read);

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Читает из памяти Double по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Double ReadDouble(Int32 address)
        {
            int read; var buffer = new byte[8];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, buffer.Length, out read);

            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Читает из памяти String по указанному адресу с заданной длиной.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public String ReadString_Unicode(Int32 address, Int32 length)
        {
            int read; var buffer = new byte[length];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, length, out read);

            var enc = new UnicodeEncoding();
            var rtnStr = enc.GetString(buffer);

            return (rtnStr.IndexOf('\0') != -1) ? rtnStr.Substring(0, rtnStr.IndexOf('\0')) : rtnStr;
        }

        /// <summary>
        /// Читает из памяти String по указанному адресу с заданной длиной в кодировке ANSCII.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public String ReadString_ASCII(Int32 address, Int32 length)
        {
            int read; var buffer = new byte[length];

            WinApi.ReadProcessMemory(OpenedProcessHandle, address, buffer, length, out read);

            var enc = new ASCIIEncoding();
            var rtnStr = enc.GetString(buffer);

            return (rtnStr.IndexOf('\0') != -1) ? rtnStr.Substring(0, rtnStr.IndexOf('\0')) : rtnStr;
        }

        /// <summary>
        /// Читает из памяти Byte, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public Int16 ChainReadByte(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadByte(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadByte(tmpInt + offsets[offsets.Length - 1]);
        }

        /// <summary>
        /// Читает из памяти Int16, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public Int16 ChainReadInt16(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadInt16(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadInt16(tmpInt + offsets[offsets.Length - 1]);
        }

        /// <summary>
        /// Читает из памяти Int32, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public Int32 ChainReadInt32(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadInt32(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadInt32(tmpInt + offsets[offsets.Length - 1]);
        }

        /// <summary>
        /// Читает из памяти UInt32, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public UInt32 ChainReadUInt32(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadUInt32(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadUInt32(tmpInt + offsets[offsets.Length - 1]);
        }
        /// <summary>
        /// Читает из памяти Int64, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public Int64 ChainReadInt64(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadInt64(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadInt64(tmpInt + offsets[offsets.Length - 1]);
        }

        /// <summary>
        /// Читает из памяти Float, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public Single ChainReadFloat(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadFloat(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadFloat(tmpInt + offsets[offsets.Length - 1]);
        }

        /// <summary>
        /// Читает из памяти Double, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public Double ChainReadDouble(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadDouble(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadDouble(tmpInt + offsets[offsets.Length - 1]);
        }

        /// <summary>
        /// Читает из памяти String заданной длины, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public String ChainReadString_Unicode(Int32 address, Int32 length, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadString_Unicode(address, length);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadString_Unicode(tmpInt + offsets[offsets.Length - 1], length);
        }

        /// <summary>
        /// Читает из памяти String заданной длины, в кодировке ASCII, используя цепочку оффсетов .
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public String ChainReadString_ASCII(Int32 address, Int32 length, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadString_ASCII(address, length);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return ReadString_ASCII(tmpInt + offsets[offsets.Length - 1], length);
        }

        /// <summary>
        /// Получает конечный адрес, используя цепочку оффсетов.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        private Int32 GetaddressByChaing(Int32 address, params Int32[] offsets)
        {
            if (offsets.Length == 0) return ReadInt32(address);

            var tmpInt = ReadInt32(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                tmpInt = ReadInt32(tmpInt + offsets[i]);
            }

            return tmpInt + offsets[offsets.Length - 1];
        }

        public T Read<T>(Int32 address)
        {
            return ReadArray<T>(address, 1)[0];
        }

        public T[] ReadArray<T>(Int32 address, int length)
        {
            T[] result = new T[length];
            GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                int read = 0;
                WinApi.ReadProcessMemory2(OpenedProcessHandle, address, ptr, length * Marshal.SizeOf(typeof(T)), out read);
            }
            finally
            {
                handle.Free();
            }
            return result;
        }

        /// <summary>
        /// Записывает Byte по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteByte(Int32 address, byte value)
        {
            int tmpInt;

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, new[] { value }, 1, out tmpInt);
        }

        /// <summary>
        /// Записывает массив Byte по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteBytes(Int32 address, byte[] value)
        {
            int tmpInt;

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, value, value.Length, out tmpInt);
        }

        /// <summary>
        /// Записывает Int16 по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteInt16(Int32 address, Int16 value)
        {
            int tmpInt;

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, BitConverter.GetBytes(value), 2, out tmpInt);
        }

        /// <summary>
        /// Записывает Int32 по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteInt32(Int32 address, Int32 value)
        {
            int tmpInt;

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, BitConverter.GetBytes(value), 4, out tmpInt);
        }

        /// <summary>
        /// Записывает Int64 по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteInt64(Int32 address, Int64 value)
        {
            int tmpInt;

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, BitConverter.GetBytes(value), 8, out tmpInt);
        }

        /// <summary>
        /// Записывает Float по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteFloat(Int32 address, float value)
        {
            int tmpInt;

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, BitConverter.GetBytes(value), 4, out tmpInt);
        }

        /// <summary>
        /// Записывает Double по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteDouble(Int32 address, Double value)
        {
            int tmpInt;

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, BitConverter.GetBytes(value), 8, out tmpInt);
        }

        /// <summary>
        /// Записывает UnicodeString по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool WriteString_Unicode(Int32 address, string str)
        {
            int tmpInt;
            var strBytes = new List<byte>();
            strBytes.AddRange(Encoding.Unicode.GetBytes(str));
            strBytes.AddRange(Encoding.Unicode.GetBytes("\0"));

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, strBytes.ToArray(), strBytes.Count, out tmpInt);
        }

        /// <summary>
        /// Записывает ASCII String по указанному адресу.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool WriteString_ASCII(Int32 address, string str)
        {
            int tmpInt;
            var strBytes = new List<byte>();
            strBytes.AddRange(Encoding.ASCII.GetBytes(str));
            strBytes.AddRange(Encoding.ASCII.GetBytes("\0"));

            return WinApi.WriteProcessMemory(OpenedProcessHandle, address, strBytes.ToArray(), strBytes.Count, out tmpInt);
        }

        /// <summary>
        /// Записывает Byte по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteByte(byte value, Int32 address, params Int32[] offsets)
        {
            return WriteByte(GetaddressByChaing(address, offsets), value);
        }

        /// <summary>
        /// Записывает массив Byte по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteBytes(byte[] value, Int32 address, params Int32[] offsets)
        {
            return WriteBytes(GetaddressByChaing(address, offsets), value);
        }

        /// <summary>
        /// Записывает Int16 по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteInt16(Int16 value, Int32 address, params Int32[] offsets)
        {
            return WriteInt16(GetaddressByChaing(address, offsets), value);
        }

        /// <summary>
        /// Записывает Int32 по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteInt32(Int32 value, Int32 address, params Int32[] offsets)
        {
            return WriteInt32(GetaddressByChaing(address, offsets), value);
        }

        /// <summary>
        /// Записывает Int64 по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteInt64(Int64 value, Int32 address, params Int32[] offsets)
        {
            return WriteInt64(GetaddressByChaing(address, offsets), value);
        }

        /// <summary>
        /// Записывает Float по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteFloat(Single value, Int32 address, params Int32[] offsets)
        {
            return WriteFloat(GetaddressByChaing(address, offsets), value);
        }

        /// <summary>
        /// Записывает Double по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="value"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteDouble(Double value, Int32 address, params Int32[] offsets)
        {
            return WriteDouble(GetaddressByChaing(address, offsets), value);
        }

        /// <summary>
        /// Записывает UnicodeString по адресу, полученному благодаря цепочке оффсетов..
        /// </summary>
        /// <param name="str"></param>
        /// <param name="address"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public bool ChainWriteString(string str, Int32 address, params Int32[] offsets)
        {
            return WriteString_Unicode(GetaddressByChaing(address, offsets), str);
        }
    }
}
