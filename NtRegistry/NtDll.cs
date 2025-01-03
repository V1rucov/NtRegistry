using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NTRegistry
{
	public static class NtDll
	{
		[Flags]
		public enum StandardRights : uint
		{
			Delete = 0x00010000,
			ReadControl = 0x00020000,
			WriteDac = 0x00040000,
			WriteOwner = 0x00080000,
			Synchronize = 0x00100000,
			Required = 0x000f0000,
			Read = ReadControl,
			Write = ReadControl,
			Execute = ReadControl,
			All = 0x001f0000,

			SpecificRightsAll = 0x0000ffff,
			AccessSystemSecurity = 0x01000000,
			MaximumAllowed = 0x02000000,
			GenericRead = 0x80000000,
			GenericWrite = 0x40000000,
			GenericExecute = 0x20000000,
			GenericAll = 0x10000000
		}

		[Flags]
		public enum KeyAccess : uint
		{
			QueryValue = 0x0001,
			SetValue = 0x0002,
			CreateSubKey = 0x0004,
			EnumerateSubKeys = 0x0008,
			Notify = 0x0010,
			CreateLink = 0x0020,
			Wow64_32Key = 0x0200,
			Wow64_64Key = 0x0100,
			Wow64_Res = 0x0300,

			GenericRead = (StandardRights.Read | QueryValue | EnumerateSubKeys | Notify) & ~StandardRights.Synchronize,
			GenericWrite = (StandardRights.Write | SetValue | CreateSubKey) & ~StandardRights.Synchronize,
			GenericExecute = GenericRead & ~StandardRights.Synchronize,

			All = (StandardRights.All | QueryValue | SetValue | CreateSubKey | EnumerateSubKeys | Notify | CreateLink) & ~StandardRights.Synchronize,

			Read = QueryValue | EnumerateSubKeys,
			Write = SetValue | CreateSubKey | Notify,
			ReadWrite = Read | Write
		}

		[Flags]
		public enum ObjectFlags : uint
		{
			Inherit = 0x2,
			Permanent = 0x10,
			Exclusive = 0x20,
			CaseInsensitive = 0x40,
			OpenIf = 0x80,
			OpenLink = 0x100,
			KernelHandle = 0x200,
			ForceAccessCheck = 0x400,
			ValidAttributes = 0x7f2
		}

		public enum KeyInformationClass : int
		{
			KeyBasicInformation,
			KeyNodeInformation,
			KeyFullInformation,
			KeyNameInformation,
			KeyCachedInformation,
			KeyFlagsInformation,
			MaxKeyInfoClass
		}

		public enum KeyValueInformationClass : int
		{
			KeyValueBasicInformation = 0,
			KeyValueFullInformation = 1,
			KeyValuePartialInformation = 2,
			KeyValueFullInformationAlign64 = 3,
			KeyValuePartialInformationAlign64 = 4,
			MaxKeyValueInfoClass = 5 
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ObjectAttributes
		{
			public int Length;
			public IntPtr RootDirectory;
			public IntPtr ObjectName;
			public ObjectFlags Attributes;
			public IntPtr SecurityDescriptor;
			public IntPtr SecurityQualityOfService;

			public ObjectAttributes(IntPtr RootDirectory, IntPtr ObjectName, ObjectFlags Attributes)
			{
				this.Length = Marshal.SizeOf(typeof(ObjectAttributes));
				this.RootDirectory = RootDirectory;
				this.ObjectName = ObjectName;
				this.Attributes = Attributes;
				this.SecurityDescriptor = IntPtr.Zero;
				this.SecurityQualityOfService = IntPtr.Zero;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct UnicodeString : IDisposable
		{
			public ushort Length;
			public ushort MaximumLength;
			public IntPtr Buffer;

			public UnicodeString(string s)
			{
				this.Length = (ushort)(s.Length * 2);
				this.MaximumLength = this.Length;
				this.Buffer = Marshal.StringToHGlobalUni(s);
			}

			public void Dispose()
			{
				Marshal.FreeHGlobal(this.Buffer);
			}
		}

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtQueryKey(
			[In] IntPtr KeyHandle,
			[In] KeyInformationClass KeyInformationClass,
			[In] IntPtr KeyInformation,
			[In] uint Length,
			[Out] out uint ResultLength
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtCreateKey(
			[Out] out IntPtr KeyHandle,
			[In] KeyAccess DesiredAccess,
			[In] ref ObjectAttributes ObjectAttributes,
			[In] uint TitleIndex,
			[In] IntPtr Class,
			[In] uint CreateOptions,
			[Out] out uint Disposition
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtOpenKey(
			[Out] out IntPtr KeyHandle,
			[In] KeyAccess DesiredAccess,
			[In] ref ObjectAttributes ObjectAttributes
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtDeleteKey(
			[In] IntPtr KeyHandle
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtEnumerateKey(
			[In] IntPtr KeyHandle,
			[In] uint Index,
			[In] KeyInformationClass KeyInformationClass,
			[In] IntPtr KeyInformation,
			[In] uint Length,
			[Out] out uint ResultLength
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtEnumerateValueKey(
			[In] IntPtr KeyHandle,
			[In] uint Index,
			[In] KeyValueInformationClass KeyValueInformationClass,
			[In] IntPtr KeyValueInformation,
			[In] uint Length,
			[Out] out uint ResultLength
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtQueryValueKey(
			[In] IntPtr KeyHandle,
			[In] ref UnicodeString ValueName,
			[In] KeyValueInformationClass KeyValueInformationClass,
			[In] IntPtr KeyValueInformation,
			[In] uint Length,
			[Out] out uint ResultLength
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtSetValueKey(
			[In] IntPtr KeyHandle,
			[In] ref UnicodeString ValueName,
			[In] uint TitleIndex,
			[In] uint Type,
			[In] IntPtr Data,
			[In] uint DataSize
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtDeleteValueKey(
			[In] IntPtr KeyHandle,
			[In] ref UnicodeString ValueName
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtClose(
			[In] IntPtr Handle
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus NtFlushKey(
			[In] IntPtr Handle
		);

		[DllImport("ntdll.dll")]
		public static extern NtStatus RtlOpenCurrentUser(
			[In] KeyAccess DesiredAccess,
			[Out] out IntPtr KeyHandle
		);


	}
}
