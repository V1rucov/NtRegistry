using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace NTRegistry
{
	public static class NtRegistry
	{
		public static NtRegistryKey ClassesRoot
		{
			get
			{
				return NtRegistryKey.OpenBaseKey(NtRegistryHive.ClassesRoot, true);
			}
		}

		public static NtRegistryKey CurrentConfig
		{
			get
			{
				return NtRegistryKey.OpenBaseKey(NtRegistryHive.CurrentConfig, true);
			}
		}

		public static NtRegistryKey CurrentUser
		{
			get
			{
				return NtRegistryKey.OpenBaseKey(NtRegistryHive.CurrentUser, true);
			}
		}

		public static NtRegistryKey DynData
		{
			get
			{
				return NtRegistryKey.OpenBaseKey(NtRegistryHive.DynData, true);
			}
		}

		public static NtRegistryKey LocalMachine
		{
			get
			{
				return NtRegistryKey.OpenBaseKey(NtRegistryHive.LocalMachine, true);
			}
		}

		public static NtRegistryKey PerformanceData
		{
			get
			{
				return NtRegistryKey.OpenBaseKey(NtRegistryHive.PerformanceData, true);
			}
		}

		public static NtRegistryKey Users
		{
			get
			{
				return NtRegistryKey.OpenBaseKey(NtRegistryHive.Users, true);
			}
		}
	}

	public class NtRegistryKey
	{
		private IntPtr key_handle = IntPtr.Zero;

		private bool is_root = false;

		private string name = null;
		private int value_count = -1;
		private int subkey_count = -1;

		public NtRegistryKey() { }

		private NtRegistryKey(IntPtr handle)  : this(handle, false) {}

		private NtRegistryKey(IntPtr handle, bool is_root)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentException("handle");

			this.key_handle = handle;

			this.is_root = is_root;
		}

		/// <summary>
		/// Opens a new NtRegistryKey that represents the requested key on the local machine.
		/// </summary>
		public static NtRegistryKey OpenBaseKey(NtRegistryHive hKey, bool writable = true)
		{
			IntPtr KeyHandle = IntPtr.Zero;
			NtStatus status;

			switch (hKey)
			{
				case NtRegistryHive.ClassesRoot:
				case NtRegistryHive.LocalMachine:
				case NtRegistryHive.PerformanceData:
				case NtRegistryHive.Users:
				case NtRegistryHive.CurrentConfig:

					string path = "";

					switch (hKey)
					{
						case NtRegistryHive.ClassesRoot:
							path = @"\Registry\Machine\Software\Classes\";
							break;

						case NtRegistryHive.LocalMachine:
							path = @"\Registry\Machine\";
							break;

						case NtRegistryHive.Users:
							path = @"\Registry\User\";
							break;

						case NtRegistryHive.CurrentConfig:
							path = @"\Registry\Machine\System\CurrentControlSet\Hardware Profiles\Current";
							break;

						case NtRegistryHive.PerformanceData:
							throw new Exception("Unsupported Hive!");

					}

					var name_us = new NtDll.UnicodeString(path);
					var name_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(name_us));
					Marshal.StructureToPtr(name_us, name_ptr, true);

					var objAttr = new NtDll.ObjectAttributes(IntPtr.Zero, name_ptr, NtDll.ObjectFlags.CaseInsensitive);

					status = NtDll.NtOpenKey(out KeyHandle, NtDll.KeyAccess.All,  ref objAttr);

					Marshal.FreeHGlobal(name_ptr);

					status.ThrowIfNotSuccess();

					break;

				case NtRegistryHive.CurrentUser:
					status = NtDll.RtlOpenCurrentUser(NtDll.KeyAccess.All, out KeyHandle);
					
					status.ThrowIfNotSuccess();

					break;

				default:
					return null;
			}

			return new NtRegistryKey(KeyHandle);
		}

		/// <summary>
		/// Returns true if this is root key (eg HKEY_LOCAL_MACHINE, HKEY_CURRENT_USER, etc)
		/// </summary>
		public bool IsRoot
		{
			get
			{
				ThrowIfClosed();

				return is_root;
			}
		}

		/// <summary>
		/// Retrieves the name of the key
		/// </summary>
		public string Name
		{
			get
			{
				ThrowIfClosed();


				// Query the name
				IntPtr KeyNameInfo = Marshal.AllocHGlobal(256);
				uint ResultLength = 0;
				var status = NtDll.NtQueryKey(this.key_handle, NtDll.KeyInformationClass.KeyNameInformation, KeyNameInfo, 256, out ResultLength);

				status.ThrowIfNotSuccess();

				var len_ptr = KeyNameInfo;
				var len = Marshal.ReadInt32(len_ptr);
				var name_ptr = IntPtr.Add(KeyNameInfo, 4);
				this.name = Marshal.PtrToStringUni(name_ptr, len / 2);

				Marshal.FreeHGlobal(KeyNameInfo);

				return name;
			}
		}

		private void FetchNameAndSubkeyCountAndValueCount()
		{
			// Query SubKeys & Values
			IntPtr KeyNameInfo = Marshal.AllocHGlobal(256);
			uint ResultLength = 0;
			var status = NtDll.NtQueryKey(this.key_handle, NtDll.KeyInformationClass.KeyFullInformation, KeyNameInfo, 256, out ResultLength);

			status.ThrowIfNotSuccess();

			var subkeys_ptr = IntPtr.Add(KeyNameInfo, 20);
			this.subkey_count = Marshal.ReadInt32(subkeys_ptr);

			var values_ptr = IntPtr.Add(KeyNameInfo, 32);
			this.value_count = Marshal.ReadInt32(values_ptr);

			Marshal.FreeHGlobal(KeyNameInfo);
		}

		/// <summary>
		/// Retrieves the count of values in the key
		/// </summary>
		public int ValueCount
		{
			get
			{
				ThrowIfClosed();

				FetchNameAndSubkeyCountAndValueCount();

				return value_count;
			}
		}

		/// <summary>
		/// Retrieves an array of strings that contains all the value names associated with this key
		/// </summary>
		/// <returns></returns>
		public string[] GetValueNames()
		{
			ThrowIfClosed();

			var valueNames = new List<string>();

			IntPtr KeyValueInfo = Marshal.AllocHGlobal(256);
			uint ResultLength = 0;

			for (uint i = 0; i < this.ValueCount; i++)
			{
				var status = NtDll.NtEnumerateValueKey(this.key_handle, i, NtDll.KeyValueInformationClass.KeyValueBasicInformation, KeyValueInfo, 256, out ResultLength);

				if (!status.IsSuccess())
					continue;

				var kind_ptr = IntPtr.Add(KeyValueInfo, 4);
				var kind = (NtRegistryValueKind)Marshal.ReadInt32(kind_ptr);
				var len_ptr = IntPtr.Add(KeyValueInfo, 8);
				var len = Marshal.ReadInt32(len_ptr);
				var name_ptr = IntPtr.Add(KeyValueInfo, 12);
				var name = Marshal.PtrToStringUni(name_ptr, len / 2);

				valueNames.Add(name);
			}

			Marshal.FreeHGlobal(KeyValueInfo);

			return valueNames.ToArray();
		}

		/// <summary>
		/// Determines if a value with specified name already exists
		/// </summary>
		public bool HasValue(string name)
		{
			ThrowIfClosed();

			return GetValueKind(name) != NtRegistryValueKind.None;
		}

		/// <summary>
		/// Retrieves the registry data type of the value associated with the specified name
		/// </summary>
		/// <returns>Kind of the value if it exists, NtRegistryValueKind.None otherwise</returns>
		public NtRegistryValueKind GetValueKind(string name)
		{
			ThrowIfClosed();

			var name_us = new NtDll.UnicodeString(name);
			var KeyValueInfo = Marshal.AllocHGlobal(256);
			uint ResultLength = 0;

			var status = NtDll.NtQueryValueKey(this.key_handle, ref name_us, NtDll.KeyValueInformationClass.KeyValueBasicInformation, KeyValueInfo, 256, out ResultLength);

			var type_ptr = IntPtr.Add(KeyValueInfo, 4);
			var type = (NtRegistryValueKind) Marshal.ReadInt32(type_ptr);

			Marshal.FreeHGlobal(KeyValueInfo);

			if (status == NtStatus.ObjectNameNotFound)
				return NtRegistryValueKind.None;
			else
				status.ThrowIfNotSuccess();

			return type;
		}

		/// <summary>
		/// Retrieves the value associated with the specified name. Returns null if the name/value pair does not exist in the registry
		/// </summary>
		/// <returns>Value associated with the name if it exists, null otherwise</returns>
		public object GetValue(string name)
		{
			ThrowIfClosed();

			IntPtr KeyValueInfo = Marshal.AllocHGlobal(1024);
			uint ResultLength = 0;

			var name_us = new NtDll.UnicodeString(name);
			
			var status = NtDll.NtQueryValueKey(this.key_handle, ref name_us, NtDll.KeyValueInformationClass.KeyValuePartialInformation, KeyValueInfo, 1024, out ResultLength);

			if (status == NtStatus.ObjectNameNotFound)
				return null;
			else
				status.ThrowIfNotSuccess();

			var type_ptr = IntPtr.Add(KeyValueInfo, 4);
			var type = (NtRegistryValueKind)Marshal.ReadInt32(type_ptr);

			var len_ptr = IntPtr.Add(KeyValueInfo, 8);
			var len = Marshal.ReadInt32(len_ptr);

			var data_ptr = IntPtr.Add(KeyValueInfo, 12);
			byte[] data = new byte[len];
			Marshal.Copy(data_ptr, data, 0, len);

			Marshal.FreeHGlobal(KeyValueInfo);

			switch (type)
			{
				case NtRegistryValueKind.Binary:
					return data;

				case NtRegistryValueKind.DWord:
					var data_dword = BitConverter.ToUInt32(data, 0);
					return data_dword;

				case NtRegistryValueKind.QWord:
					var data_qword = BitConverter.ToUInt64(data, 0);
					return data_qword;

				case NtRegistryValueKind.String:
				case NtRegistryValueKind.ExpandedString:
					var data_str = UnicodeEncoding.Unicode.GetString(data);
					return data_str;

				case NtRegistryValueKind.MultiString:
					var strings = new List<string>();
					var last_index = 0;
					for (var i = 0; i < data.Length; i++)
					{
						if (data[i] == '\0')
						{
							var s = UnicodeEncoding.Unicode.GetString(data, last_index, i - last_index - 1);
							last_index = i;

							strings.Add(s);
						}
					}

					return strings.ToArray();

				default:
					throw new Exception("Unknown value kind: " + type.ToString());
			}
		}

		/// <summary>
		/// Retrieves the value associated with the specified name. Returns defaultValue if the name/value pair does not exist in the registry
		/// </summary>
		public object GetValue(string name, object defaultValue)
		{
			ThrowIfClosed();

			var value = GetValue(name);

			if (value == null)
				return defaultValue;

			return value;
		}

		/// <summary>
		/// Sets the specified name/value pair
		/// </summary>
		/// <param name="name">The name of the value to store</param>
		/// <param name="value">The data to be stored</param>
		public void SetValue(string name, object value)
		{
			ThrowIfClosed();

			if (value == null)
				throw new ArgumentNullException("value");

			if (value is byte[])
				SetValue(name, value, NtRegistryValueKind.Binary);

			else if (value is uint || value is int)
				SetValue(name, (uint)value, NtRegistryValueKind.DWord);

			else if (value is UInt64 || value is Int64)
				SetValue(name, (UInt64)value, NtRegistryValueKind.QWord);

			else if (value is string)
				SetValue(name, value, NtRegistryValueKind.String);

			else if (value is string[])
				SetValue(name, value, NtRegistryValueKind.MultiString);

			else
				throw new ArgumentException("Cannot imply type from value of type " + value.GetType().ToString());
		}

		/// <summary>
		/// Sets the value of a name/value pair in the registry key, using the specified registry data type.
		/// </summary>
		/// <param name="name">The name of the value to be stored</param>
		/// <param name="value">The data to be stored</param>
		/// <param name="valueKind">The registry data type to use when storing the data</param>
		public void SetValue(string name, object value, NtRegistryValueKind valueKind)
		{
			ThrowIfClosed();

			switch (valueKind)
			{
				case NtRegistryValueKind.Binary:
					if (value is byte[])
						SetValue(name, value, NtRegistryValueKind.Binary);
					else
						throw new ArgumentException("value must be byte[] when valueKind is Binary");

					break;

				case NtRegistryValueKind.DWord:
					if (value is byte[])
						SetValue(name, value, NtRegistryValueKind.DWord);
					else if (value is int || value is uint)
						SetValue_Raw(name, BitConverter.GetBytes((uint)value), NtRegistryValueKind.DWord);
					else
						throw new ArgumentException("value must be of type Byte[] or Int32 or UInt32 when valueKind is DWord");

					break;

				case NtRegistryValueKind.QWord:
					if (value is byte[])
						SetValue(name, value, NtRegistryValueKind.QWord);
					else if (value is Int64 || value is UInt64)
						SetValue_Raw(name, BitConverter.GetBytes((UInt64)value), NtRegistryValueKind.QWord);
					else
						throw new ArgumentException("value must be of type Byte[] or Int64 or UInt64 when valueKind is QWord");

					break;

				case NtRegistryValueKind.String:
				case NtRegistryValueKind.ExpandedString:
					if (value is byte[])
						SetValue(name, value, valueKind);
					else if (value is string)
						SetValue_Raw(name, UnicodeEncoding.Unicode.GetBytes((string)value), valueKind);
					else
						throw new ArgumentException("value must be of type Byte[] or String when valueKind is String or ExpandedString");

					break;

				case NtRegistryValueKind.MultiString:
					if (value is byte[])
					{
						SetValue(name, value, NtRegistryValueKind.MultiString);
					}
					else if (value is string[])
					{
						var bytes = new List<byte>();

						foreach (var s in (value as string[]))
						{
							bytes.AddRange(UnicodeEncoding.Default.GetBytes(s));
							bytes.Add(0);
						}

						bytes.Add(0);

						SetValue(name, bytes, NtRegistryValueKind.MultiString);
					}
					else
						throw new ArgumentException("value must be of type Byte[] or String[] when valueKind is MultiString");

					break;

				case NtRegistryValueKind.Unknown:
				case NtRegistryValueKind.None:
					throw new ArgumentException("valueKind");
			}
		}

		/// <summary>
		/// Sets the value of a name/value pair in the registry key, using the specified registry data type.
		/// </summary>
		/// <param name="name">The name of the value to be stored</param>
		/// <param name="value">The data to be stored</param>
		/// <param name="valueKind">The registry data type to use when storing the data</param>
		private void SetValue_Raw(string name, byte[] value, NtRegistryValueKind kind)
		{
			ThrowIfClosed();

			var name_us = new NtDll.UnicodeString(name);

			var len = value.Length;
			var data_ptr = Marshal.AllocHGlobal(len);
			Marshal.Copy(value, 0, data_ptr, len);

			var status = NtDll.NtSetValueKey(this.key_handle, ref name_us, 0, (uint)kind, data_ptr, (uint)len);

			Marshal.FreeHGlobal(data_ptr);

			status.ThrowIfNotSuccess();
		}

		/// <summary>
		/// Deletes the specified value from this key
		/// </summary>
		/// <param name="name"></param>
		public void DeleteValue(string name)
		{
			ThrowIfClosed();

			var name_us = new NtDll.UnicodeString(name);

			var status = NtDll.NtDeleteValueKey(this.key_handle, ref name_us);

			status.ThrowIfNotSuccess();
		}

		/// <summary>
		/// Retrieves the count of subkeys of the current key
		/// </summary>
		public int SubKeyCount
		{
			get
			{
				ThrowIfClosed();

				FetchNameAndSubkeyCountAndValueCount();

				return subkey_count;
			}
		}

		/// <summary>
		/// Retrieves an array of strings that contains all the subkey names
		/// </summary>
		public string[] GetSubKeyNames()
		{
			ThrowIfClosed();

			var subKeyNames = new List<string>();

			var KeyInfo = Marshal.AllocHGlobal(1024);
			uint ResultLength = 0;

			for (uint i = 0; i < this.SubKeyCount; i++)
			{
				var status = NtDll.NtEnumerateKey(this.key_handle, i, NtDll.KeyInformationClass.KeyBasicInformation, KeyInfo, 1024, out ResultLength);

				if (!status.IsSuccess())
					continue;

				var len_ptr = IntPtr.Add(KeyInfo, 12);
				var len = Marshal.ReadInt32(len_ptr);

				var name_ptr = IntPtr.Add(KeyInfo, 16);
				var name = Marshal.PtrToStringUni(name_ptr, len / 2);

				subKeyNames.Add(name);
			}

			Marshal.FreeHGlobal(KeyInfo);

			return subKeyNames.ToArray();
		}

		/// <summary>
		/// Creates a new subkey or opens an existing subkey for write access
		/// </summary>
		public NtRegistryKey CreateSubKey(string subkey)
		{
			ThrowIfClosed();

			return CreateSubKey(subkey, NtRegistryOptions.None);
		}

		/// <summary>
		/// Creates a new subkey or opens an existing subkey for write access, with specified options
		/// </summary>
		public NtRegistryKey CreateSubKey(string subkey, NtRegistryOptions createOptions)
		{
			ThrowIfClosed();

			var subkey_us = new NtDll.UnicodeString(subkey);
			var subkey_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(subkey_us));
			Marshal.StructureToPtr(subkey_us, subkey_ptr, true);

			var objAttr = new NtDll.ObjectAttributes(key_handle, subkey_ptr, NtDll.ObjectFlags.CaseInsensitive);

			uint disposition =  0;

			IntPtr subkey_handle = IntPtr.Zero;

			var status = NtDll.NtCreateKey(out subkey_handle, NtDll.KeyAccess.All, ref objAttr, 0, IntPtr.Zero, (uint)createOptions, out disposition);

			Marshal.FreeHGlobal(subkey_ptr);

			status.ThrowIfNotSuccess();

			return new NtRegistryKey(subkey_handle);
		}

		/// <summary>
		/// Retrieves a subkey as read-only
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public NtRegistryKey OpenSubKey(string name)
		{
			return OpenSubKey(name, false);
		}

		/// <summary>
		/// Retrieves a specified subkey, and specifies whether write access is be applied to the key
		/// </summary>
		public NtRegistryKey OpenSubKey(string name, bool writable)
		{
			return OpenSubKey(name, writable ? NtDll.KeyAccess.ReadWrite : NtDll.KeyAccess.Read);
		}

		private NtRegistryKey OpenSubKey(string name, NtDll.KeyAccess keyAccess)
		{
			ThrowIfClosed();

			var name_us = new NtDll.UnicodeString(name);
			IntPtr name_ptr = Marshal.AllocHGlobal(Marshal.SizeOf(name_us));
			Marshal.StructureToPtr(name_us, name_ptr, true);

			var objAttributes = new NtDll.ObjectAttributes(this.key_handle, name_ptr, NtDll.ObjectFlags.CaseInsensitive);

			IntPtr subkey_ptr;
			var status = NtDll.NtOpenKey(out subkey_ptr, keyAccess, ref objAttributes);

			Marshal.FreeHGlobal(name_ptr);

			status.ThrowIfNotSuccess();

			return new NtRegistryKey(subkey_ptr);
		}

		/// <summary>
		/// Delete a subkey. The subkey must have no child keys.
		/// </summary>
		/// <param name="subkey"></param>
		public void DeleteSubKey(string subkey)
		{
			ThrowIfClosed();

			NtRegistryKey key = null;

			try
			{
				key = OpenSubKey(subkey, NtDll.KeyAccess.All);

				if (key.SubKeyCount > 0)
					throw new Exception("Key contains subkeys. It cannot be deleted");

			}
			catch (Exception ex)
			{
				throw new Exception("Could not open subkey '" + subkey  + "'", ex);
			}

			var status = NtDll.NtDeleteKey(key.key_handle);

			status.ThrowIfNotSuccess();

			key.key_handle = IntPtr.Zero;

			key.Close();
		}

		/// <summary>
		/// Deletes the subkey and any child subkeys recursively
		/// </summary>
		/// <param name="subkey"></param>
		public void DeleteSubKeyTree(string subkey)
		{
			ThrowIfClosed();

			var k = this.OpenSubKey(subkey);

			foreach (var subKeyName in k.GetSubKeyNames())
			{
				k.DeleteSubKeyTree(subKeyName);
			}

			this.DeleteSubKey(subkey);
		}

		/// <summary>
		/// Writes all the attributes of the specified open registry key into the registry.
		/// </summary>
		public void Flush()
		{
			ThrowIfClosed();

			if (this.key_handle == IntPtr.Zero)
				return;

			var status = NtDll.NtFlushKey(this.key_handle);

			status.ThrowIfNotSuccess();
		}

		/// <summary>
		/// Closes the key and flushes it to disk if its contents have been modified
		/// </summary>
		public void Close()
		{
			// Don't check for closed key -- simply ignore

			if (this.key_handle == IntPtr.Zero)
				return;

			Flush();

			// Close
			var status = NtDll.NtClose(this.key_handle);

			key_handle = IntPtr.Zero;
		}

		/// <summary>
		/// Disposes the current instance
		/// </summary>
		public void Dispose()
		{
			this.Close();

			// Release all objects ...
		}

		private void ThrowIfClosed()
		{
			if (this.key_handle == IntPtr.Zero)
				throw new Exception("Key has been closed");
		}

		/// <summary>
		/// Retrieves a string representation of this key.
		/// </summary>
		/// <returns>A string representing the key</returns>
		public override string ToString()
		{
			return this.name;
		}
	}

	public enum NtRegistryOptions : uint
	{
		None = 0,
		Volatile = 1
	}

	public enum NtRegistryValueKind : uint
	{
		None = 0,
		String = 1,
		ExpandedString = 2,
		Binary = 3,
		DWord = 4,
		MultiString = 7,
		QWord = 11,
		Unknown = 0xFFFFFFFF,
	}

	public static class NtRegistryValueKindExtensions
	{
		public static string CodeName(this NtRegistryValueKind kind)
		{
			switch (kind)
			{
				case NtRegistryValueKind.None:
					return "REG_NONE";

				case NtRegistryValueKind.String:
					return "REG_SZ";

				case NtRegistryValueKind.ExpandedString:
					return "REG_EXPAND_SZ";

				case NtRegistryValueKind.Binary:
					return "REG_BINARY";

				case NtRegistryValueKind.DWord:
					return "REG_DWORD";

				case NtRegistryValueKind.MultiString:
					return "REG_MULTI_SZ";

				case NtRegistryValueKind.QWord:
					return "REG_QWORD";

				case NtRegistryValueKind.Unknown:
					return "REG_UNKNOWN";

				default:
					throw new ArgumentException("New kind?? " + kind.ToString());
			}
		}
	}

	public enum NtRegistryHive
	{
		ClassesRoot,
		CurrentConfig,
		CurrentUser,
		DynData,
		LocalMachine,
		PerformanceData,
		Users
	}

	
}
