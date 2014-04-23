using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Gearman
{
	public abstract class Packet
	{
		
		protected char[] _Magic;
		
		protected PacketType _Type; 
		
		protected int _Size;
		
		protected byte[] _RawData; 
		protected byte[] _Header; 
		
		public Packet() { }
		
		public Packet(byte[] fromdata)
		{
			byte[] typebytes = fromdata.Slice(4, 8);
			byte[] sizebytes = fromdata.Slice(8, 12);
		
			if(BitConverter.IsLittleEndian)
			{
				Array.Reverse(typebytes);
				Array.Reverse(sizebytes);
			}
			
			this._Type = (PacketType)BitConverter.ToInt32(typebytes, 0);
			this._Size = BitConverter.ToInt32(sizebytes, 0);		
			this._RawData = new byte[this._Size];
			Array.Copy(fromdata, 12, this._RawData, 0, this._Size);
		}
				
		public int ParseString(int offset, ref string storage)
		{
			int pStart; 
			int pOff;
			pOff = pStart = offset; 
			for(; pOff < _RawData.Length && _RawData[pOff] != 0; pOff++);
			storage = new ASCIIEncoding().GetString(_RawData.Slice(pStart, pOff));
			// Return 1 past where we are...
			return pOff + 1;
		}
		
		public int Length 
		{ 
			set 
			{ 
			}
			
			get 
			{ 
				return this._Size + 12;
			}
		}
				
		public PacketType Type 
		{
			set 
			{ 
				Console.WriteLine("Setting type!");
				this._Type = (PacketType)value;
			}
			
			get 
			{ 
				return this._Type;
			}
		}
		
		public virtual byte[] ToByteArray() 
		{ 
			return Header;
		}

		public byte[] Header
		{				
			get 
			{
				if (_Header == null)
				{
					_Header = new byte[12];
		
					byte[] typebytes = BitConverter.GetBytes((int)this.Type);
					byte[] sizebytes;
					
					sizebytes  = BitConverter.GetBytes((int)this._Size);
					
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse(typebytes);
						Array.Reverse(sizebytes);
					}
					
					// HACK: Sooooo ugly... replace with the _Magic[] Data
					
					_Header[0] = (byte)'\0';
					_Header[1] = (byte)'R';
					_Header[2] = (byte)'E';
					_Header[3] = (byte)'Q';
					
					_Header[4] = typebytes[0];
					_Header[5] = typebytes[1];
					_Header[6] = typebytes[2];
					_Header[7] = typebytes[3];
					
					_Header[8] = sizebytes[0];
					_Header[9] = sizebytes[1];
					_Header[10] = sizebytes[2];
					_Header[11] = sizebytes[3];		
				}
				
				return _Header;
			}
			
			set {}
		}
		
		public void Dump()
		{
			try 
			{ 
				int count = 0;
				int idata; 
				byte[] line = new byte[16]; 
				
				ASCIIEncoding encoding = new ASCIIEncoding( );

				if(_RawData != null)
				{
					while(count < this._Size)
					{
						idata = _RawData[count];
						
						if ( (((count % 16) == 0) && count != 0) || (count == _RawData.Length - 1) )
						{
							string output = "";
							string text = encoding.GetString(line);
	
							string result = Regex.Replace(text, @"[^0-9a-zA-Z]", ".");
								
							for(int i = 0; i < _RawData.Length && i < 16; i+=2) 
							{		
								output += String.Format("{0:x2}{1:x2} ", line[i], line[i+1]);
							}
							
							Console.WriteLine("0x{0:x4}: {1} {2}", count-16, output, result);
							line = new byte[16];
						} 
						
						line[count % 16] = Convert.ToByte(idata);
						
						
						count++;
					}
				}
			} 
			catch
			{
			}
		}
		
		public override string ToString()
		{
			return string.Format("{0} packet. Data: {1} bytes", _Type.ToString("g"), _RawData.Length);
		}
	}
}
	
public static class Extensions
{
	/// <summary>
	/// Get the array slice between the two indexes, similar to Python or other languages' array slices.
	/// Index is inclusive for start index, exclusive for end index. (i.e 0-2 is really elements 0,1)
	/// Credit for this goes to http://dotnetperls.com/array-slice
	/// </summary>
	public static T[] Slice<T>(this T[] source, int start, int end)
	{
		// Handles negative ends
		if (end < 0)
		{
			end = source.Length - start - end - 1;
		}
		int len = end - start;

		// Return new array
		T[] res = new T[len];
		for (int i = 0; i < len; i++)
		{
			res[i] = source[i + start];
		}
		return res;
	}
}
