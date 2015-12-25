﻿
using System;
using System.Linq;
using System.IO;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;

namespace NBitcoin
{
	public class uint256 :  IBitcoinSerializable
	{
		public static uint256 Zero
		{
			get { return new uint256(0); }
		}

		public static uint256 One 
		{
			get { return new uint256(1); }
		}

		public uint256()
		{
		}

		public uint256(uint256 b)
		{
			pn0 = b.pn0;
			pn1 = b.pn1;
			pn2 = b.pn2;
			pn3 = b.pn3;
			pn4 = b.pn4;
			pn5 = b.pn5;
			pn6 = b.pn6;
			pn7 = b.pn7;
		}

		public static uint256 Parse(string hex)
		{
			var ret = new uint256();
			ret.SetHex(hex);
			return ret;
		}
		public static bool TryParse(string hex, out uint256 result)
		{
			if(hex == null)
				throw new ArgumentNullException("hex");
			result = null;
			if(hex.Length != WIDTH_BYTE * 2)
				return false;
			if(!((HexEncoder)Encoders.Hex).IsValid(hex))
				return false;
			var ret = new uint256();
			ret.SetHex(hex);
			result = ret;
			return true;
		}

		private static readonly HexEncoder Encoder = new HexEncoder();
		private const int WIDTH_BYTE = 256 / 8;
		UInt32 pn0;
		UInt32 pn1;
		UInt32 pn2;
		UInt32 pn3;
		UInt32 pn4;
		UInt32 pn5;
		UInt32 pn6;
		UInt32 pn7;
		
		internal void SetHex(string str)
		{
			pn0 = 0;
			pn1 = 0;
			pn2 = 0;
			pn3 = 0;
			pn4 = 0;
			pn5 = 0;
			pn6 = 0;
			pn7 = 0;
			str = str.Trim();

			if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
				str = str.Substring(2);

			var bytes = Encoder.DecodeData(str).Reverse().ToArray();
			SetBytes(bytes);
		}

		public byte GetByte(int index)
		{
			var uintIndex = index / sizeof(uint);
			var byteIndex = index % sizeof(uint);
			UInt32 value;
			switch(uintIndex)
			{
				case 0: 
					value = pn0;
					break;
				case 1: 
					value = pn1;
					break;
				case 2: 
					value = pn2;
					break;
				case 3: 
					value = pn3;
					break;
				case 4: 
					value = pn4;
					break;
				case 5: 
					value = pn5;
					break;
				case 6: 
					value = pn6;
					break;
				case 7: 
					value = pn7;
					break;
				default:
					throw new ArgumentOutOfRangeException("index");
			}
			return (byte)(value >> (byteIndex * 8));
		}

		private void SetBytes(byte[] arr)
		{
			pn0 = Utils.ToUInt32(arr, 4 * 0, true);
			pn1 = Utils.ToUInt32(arr, 4 * 1, true);
			pn2 = Utils.ToUInt32(arr, 4 * 2, true);
			pn3 = Utils.ToUInt32(arr, 4 * 3, true);
			pn4 = Utils.ToUInt32(arr, 4 * 4, true);
			pn5 = Utils.ToUInt32(arr, 4 * 5, true);
			pn6 = Utils.ToUInt32(arr, 4 * 6, true);
			pn7 = Utils.ToUInt32(arr, 4 * 7, true);
	
		}

		public override string ToString()
		{ 
			return Encoder.EncodeData(ToBytes().Reverse().ToArray());
		}

		public uint256(ulong b)
		{
			pn0 = (uint)b;
			pn1 = (uint)(b >> 32);
			pn2 = 0;
			pn3 = 0;
			pn4 = 0;
			pn5 = 0;
			pn6 = 0;
			pn7 = 0;
		}

		public uint256(byte[] vch, bool lendian = true)
		{
			if (vch.Length != WIDTH_BYTE)
			{
				throw new FormatException("the byte array should be 256 byte long");
			}

			if(!lendian)
				vch = vch.Reverse().ToArray();

			SetBytes(vch);
		}

		public uint256(string str)
		{
			SetHex(str);
		}

		public uint256(byte[] vch)
			:this(vch, true)
		{
		}

		public override bool Equals(object obj)
		{
			var item = obj as uint256;
			if(item == null)
				return false;
			bool equals = true;		
			equals &= pn0 == item.pn0;
			equals &= pn1 == item.pn1;
			equals &= pn2 == item.pn2;
			equals &= pn3 == item.pn3;
			equals &= pn4 == item.pn4;
			equals &= pn5 == item.pn5;
			equals &= pn6 == item.pn6;
			equals &= pn7 == item.pn7;
			return equals;
		}

		public static bool operator ==(uint256 a, uint256 b)
		{
			if(System.Object.ReferenceEquals(a, b))
				return true;
			if(((object)a == null) || ((object)b == null))
				return false;

			bool equals = true;		
			equals &= a.pn0 == b.pn0;
			equals &= a.pn1 == b.pn1;
			equals &= a.pn2 == b.pn2;
			equals &= a.pn3 == b.pn3;
			equals &= a.pn4 == b.pn4;
			equals &= a.pn5 == b.pn5;
			equals &= a.pn6 == b.pn6;
			equals &= a.pn7 == b.pn7;
			return equals;
		}

		public static bool operator <(uint256 a, uint256 b)
		{
			return Comparison(a, b) < 0;
		}

		public static bool operator >(uint256 a, uint256 b)
		{
			return Comparison(a, b) > 0;
		}

		public static bool operator <=(uint256 a, uint256 b)
		{
			return Comparison(a, b) <= 0;
		}

		public static bool operator >=(uint256 a, uint256 b)
		{
			return Comparison(a, b) >= 0;
		}

		private static int Comparison(uint256 a, uint256 b)
		{
			if (a.pn7 < b.pn7)
				return -1;
			if (a.pn7 > b.pn7)
				return 1;
			if (a.pn6 < b.pn6)
				return -1;
			if (a.pn6 > b.pn6)
				return 1;
			if (a.pn5 < b.pn5)
				return -1;
			if (a.pn5 > b.pn5)
				return 1;
			if (a.pn4 < b.pn4)
				return -1;
			if (a.pn4 > b.pn4)
				return 1;
			if (a.pn3 < b.pn3)
				return -1;
			if (a.pn3 > b.pn3)
				return 1;
			if (a.pn2 < b.pn2)
				return -1;
			if (a.pn2 > b.pn2)
				return 1;
			if (a.pn1 < b.pn1)
				return -1;
			if (a.pn1 > b.pn1)
				return 1;
			if (a.pn0 < b.pn0)
				return -1;
			if (a.pn0 > b.pn0)
				return 1;
			return 0;
		}

		public static bool operator !=(uint256 a, uint256 b)
		{
			return !(a == b);
		}

		public static bool operator ==(uint256 a, ulong b)
		{
			return (a == new uint256(b));
		}

		public static bool operator !=(uint256 a, ulong b)
		{
			return !(a == new uint256(b));
		}

		public static implicit operator uint256(ulong value)
		{
			return new uint256(value);
		}

		
		public byte[] ToBytes(bool lendian = true)
		{
			var arr = new byte[WIDTH_BYTE];
			Buffer.BlockCopy(Utils.ToBytes(pn0, true), 0, arr, 4 * 0, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn1, true), 0, arr, 4 * 1, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn2, true), 0, arr, 4 * 2, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn3, true), 0, arr, 4 * 3, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn4, true), 0, arr, 4 * 4, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn5, true), 0, arr, 4 * 5, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn6, true), 0, arr, 4 * 6, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn7, true), 0, arr, 4 * 7, 4);
			if (!lendian)
				Array.Reverse(arr);
			return arr;
		}

		public void ReadWrite(BitcoinStream stream)
		{
			if(stream.Serializing)
			{
				var b = ToBytes();
				stream.ReadWrite(ref b);
			}
			else
			{
				byte[] b = new byte[WIDTH_BYTE];
				stream.ReadWrite(ref b);
				this.SetBytes(b);
			}
		}

		public int GetSerializeSize(int nType=0, ProtocolVersion protocolVersion = ProtocolVersion.PROTOCOL_VERSION)
		{
			return WIDTH_BYTE;
		}

		public int Size
		{
			get
			{
				return WIDTH_BYTE;
			}
		}

		public ulong GetLow64()
		{
			return pn0 | (ulong)pn1 << 32;
		}

		public uint GetLow32()
		{
			return pn0;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			unchecked
			{
				hash = hash * 31 + pn0.GetHashCode();
				hash = hash * 31 + pn1.GetHashCode();
				hash = hash * 31 + pn2.GetHashCode();
				hash = hash * 31 + pn3.GetHashCode();
				hash = hash * 31 + pn4.GetHashCode();
				hash = hash * 31 + pn5.GetHashCode();
				hash = hash * 31 + pn6.GetHashCode();
				hash = hash * 31 + pn7.GetHashCode();
			}
			return hash;
		}
	}
	public class uint160 :  IBitcoinSerializable
	{
		public static uint160 Zero
		{
			get { return new uint160(0); }
		}

		public static uint160 One 
		{
			get { return new uint160(1); }
		}

		public uint160()
		{
		}

		public uint160(uint160 b)
		{
			pn0 = b.pn0;
			pn1 = b.pn1;
			pn2 = b.pn2;
			pn3 = b.pn3;
			pn4 = b.pn4;
		}

		public static uint160 Parse(string hex)
		{
			var ret = new uint160();
			ret.SetHex(hex);
			return ret;
		}
		public static bool TryParse(string hex, out uint160 result)
		{
			if(hex == null)
				throw new ArgumentNullException("hex");
			result = null;
			if(hex.Length != WIDTH_BYTE * 2)
				return false;
			if(!((HexEncoder)Encoders.Hex).IsValid(hex))
				return false;
			var ret = new uint160();
			ret.SetHex(hex);
			result = ret;
			return true;
		}

		private static readonly HexEncoder Encoder = new HexEncoder();
		private const int WIDTH_BYTE = 160 / 8;
		UInt32 pn0;
		UInt32 pn1;
		UInt32 pn2;
		UInt32 pn3;
		UInt32 pn4;
		
		internal void SetHex(string str)
		{
			pn0 = 0;
			pn1 = 0;
			pn2 = 0;
			pn3 = 0;
			pn4 = 0;
			str = str.Trim();

			if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
				str = str.Substring(2);

			var bytes = Encoder.DecodeData(str).Reverse().ToArray();
			SetBytes(bytes);
		}

		public byte GetByte(int index)
		{
			var uintIndex = index / sizeof(uint);
			var byteIndex = index % sizeof(uint);
			UInt32 value;
			switch(uintIndex)
			{
				case 0: 
					value = pn0;
					break;
				case 1: 
					value = pn1;
					break;
				case 2: 
					value = pn2;
					break;
				case 3: 
					value = pn3;
					break;
				case 4: 
					value = pn4;
					break;
				default:
					throw new ArgumentOutOfRangeException("index");
			}
			return (byte)(value >> (byteIndex * 8));
		}

		private void SetBytes(byte[] arr)
		{
			pn0 = Utils.ToUInt32(arr, 4 * 0, true);
			pn1 = Utils.ToUInt32(arr, 4 * 1, true);
			pn2 = Utils.ToUInt32(arr, 4 * 2, true);
			pn3 = Utils.ToUInt32(arr, 4 * 3, true);
			pn4 = Utils.ToUInt32(arr, 4 * 4, true);
	
		}

		public override string ToString()
		{ 
			return Encoder.EncodeData(ToBytes().Reverse().ToArray());
		}

		public uint160(ulong b)
		{
			pn0 = (uint)b;
			pn1 = (uint)(b >> 32);
			pn2 = 0;
			pn3 = 0;
			pn4 = 0;
		}

		public uint160(byte[] vch, bool lendian = true)
		{
			if (vch.Length != WIDTH_BYTE)
			{
				throw new FormatException("the byte array should be 160 byte long");
			}

			if(!lendian)
				vch = vch.Reverse().ToArray();

			SetBytes(vch);
		}

		public uint160(string str)
		{
			SetHex(str);
		}

		public uint160(byte[] vch)
			:this(vch, true)
		{
		}

		public override bool Equals(object obj)
		{
			var item = obj as uint160;
			if(item == null)
				return false;
			bool equals = true;		
			equals &= pn0 == item.pn0;
			equals &= pn1 == item.pn1;
			equals &= pn2 == item.pn2;
			equals &= pn3 == item.pn3;
			equals &= pn4 == item.pn4;
			return equals;
		}

		public static bool operator ==(uint160 a, uint160 b)
		{
			if(System.Object.ReferenceEquals(a, b))
				return true;
			if(((object)a == null) || ((object)b == null))
				return false;

			bool equals = true;		
			equals &= a.pn0 == b.pn0;
			equals &= a.pn1 == b.pn1;
			equals &= a.pn2 == b.pn2;
			equals &= a.pn3 == b.pn3;
			equals &= a.pn4 == b.pn4;
			return equals;
		}

		public static bool operator <(uint160 a, uint160 b)
		{
			return Comparison(a, b) < 0;
		}

		public static bool operator >(uint160 a, uint160 b)
		{
			return Comparison(a, b) > 0;
		}

		public static bool operator <=(uint160 a, uint160 b)
		{
			return Comparison(a, b) <= 0;
		}

		public static bool operator >=(uint160 a, uint160 b)
		{
			return Comparison(a, b) >= 0;
		}

		private static int Comparison(uint160 a, uint160 b)
		{
			if (a.pn4 < b.pn4)
				return -1;
			if (a.pn4 > b.pn4)
				return 1;
			if (a.pn3 < b.pn3)
				return -1;
			if (a.pn3 > b.pn3)
				return 1;
			if (a.pn2 < b.pn2)
				return -1;
			if (a.pn2 > b.pn2)
				return 1;
			if (a.pn1 < b.pn1)
				return -1;
			if (a.pn1 > b.pn1)
				return 1;
			if (a.pn0 < b.pn0)
				return -1;
			if (a.pn0 > b.pn0)
				return 1;
			return 0;
		}

		public static bool operator !=(uint160 a, uint160 b)
		{
			return !(a == b);
		}

		public static bool operator ==(uint160 a, ulong b)
		{
			return (a == new uint160(b));
		}

		public static bool operator !=(uint160 a, ulong b)
		{
			return !(a == new uint160(b));
		}

		public static implicit operator uint160(ulong value)
		{
			return new uint160(value);
		}

		
		public byte[] ToBytes(bool lendian = true)
		{
			var arr = new byte[WIDTH_BYTE];
			Buffer.BlockCopy(Utils.ToBytes(pn0, true), 0, arr, 4 * 0, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn1, true), 0, arr, 4 * 1, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn2, true), 0, arr, 4 * 2, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn3, true), 0, arr, 4 * 3, 4);
			Buffer.BlockCopy(Utils.ToBytes(pn4, true), 0, arr, 4 * 4, 4);
			if (!lendian)
				Array.Reverse(arr);
			return arr;
		}

		public void ReadWrite(BitcoinStream stream)
		{
			if(stream.Serializing)
			{
				var b = ToBytes();
				stream.ReadWrite(ref b);
			}
			else
			{
				byte[] b = new byte[WIDTH_BYTE];
				stream.ReadWrite(ref b);
				this.SetBytes(b);
			}
		}

		public int GetSerializeSize(int nType=0, ProtocolVersion protocolVersion = ProtocolVersion.PROTOCOL_VERSION)
		{
			return WIDTH_BYTE;
		}

		public int Size
		{
			get
			{
				return WIDTH_BYTE;
			}
		}

		public ulong GetLow64()
		{
			return pn0 | (ulong)pn1 << 32;
		}

		public uint GetLow32()
		{
			return pn0;
		}

		public override int GetHashCode()
		{
			int hash = 17;
			unchecked
			{
				hash = hash * 31 + pn0.GetHashCode();
				hash = hash * 31 + pn1.GetHashCode();
				hash = hash * 31 + pn2.GetHashCode();
				hash = hash * 31 + pn3.GetHashCode();
				hash = hash * 31 + pn4.GetHashCode();
			}
			return hash;
		}
	}
}