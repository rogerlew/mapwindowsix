//********************************************************************************************************
// Product Name: MapWindow.Data.IO.dll Alpha
// Description:  The core libraries for the MapWindow 6.0 project.
//
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.Data.IO.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February 2008
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MapWindow.Main;
namespace MapWindow.Data.IO
{


    /// <summary>
    /// The buffered binary reader was originally designed by Ted Dunsford to make shapefile reading more
    /// efficient, but ostensibly could be used for other binary reading exercises.  To use this class,
    /// simply specify the BufferSize in bytes that you would like to use and begin reading values.
    /// </summary>
    public class BufferedBinaryWriter
    {
        #region Private Variables

        private BinaryWriter _binaryWriter;
        private FileStream _fileStream;
        private long _fileLength;
        private long _fileOffset; // position in the entire file
        private byte[] _buffer;
        private int _bufferSize;
        private int _maxBufferSize = 9600000; // Approximately around ten megs, divisible by 8 
        private long _bufferOffset; // Position of the start of the buffer relative to the start of the file
        private bool _isBufferLoaded;       
        private int _writeOffset; // 
        private ProgressMeter _progressMeter;
        private IProgressHandler _progressHandler; 

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of BufferedBinaryReader.
        /// </summary>
        ///<param name="filename">The string path of a file to open using this BufferedBinaryReader.</param>
        public BufferedBinaryWriter(string filename):this(filename, null, 100000)
        {
            // This is just an overload that sends the default null value in for the progressHandler
            _progressHandler = Components.DataManager.DefaultDataManager.ProgressHandler;
            _progressMeter = new ProgressMeter(_progressHandler);
        }

        /// <summary>
        /// Creates a new instance of BufferedBinaryWriter, and specifies where to send progress messages.
        /// </summary>
        /// <param name="filename">The string path of a file to open using this BufferedBinaryReader.</param>
        /// <param name="progressHandler">Any implementation of IProgressHandler for receiving progress messages.</param>
        /// <param name="expectedByteCount">A long specifying the number of bytes that will be written for the purposes of tracking progress</param>
        public BufferedBinaryWriter(string filename, IProgressHandler progressHandler, long expectedByteCount)
        {
           
            
            if (File.Exists(filename))
            {
                // Imagine we have written a header and want to add more stuff
                _fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write);
                FileInfo fi = new FileInfo(filename);
                _fileLength = fi.Length;
                _fileOffset = 0;
            }
            else
            {
                // In this case, we just create the new file from scratch
                _fileStream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);
                _fileLength = 0;
                _fileOffset = 0;
            }
            
            _binaryWriter = new BinaryWriter(_fileStream);
            _buffer = new byte[expectedByteCount];
            _bufferSize = Convert.ToInt32(expectedByteCount);
            _maxBufferSize = _bufferSize;
            _writeOffset = -1; // There is no buffer loaded.
            _bufferOffset = -1; // -1 means no buffer is loaded.

            ProgressHandler = progressHandler;
            ProgressMeter.Key = "Writing to " + System.IO.Path.GetFileName(filename);
            ProgressMeter.EndValue = expectedByteCount;

            //_progressMeter = new ProgressMeter(progressHandler, "Writing to " + System.IO.Path.GetFileName(filename), expectedByteCount);
            //if (expectedByteCount < 1000000) _progressMeter.StepPercent = 5;
            //if (expectedByteCount < 5000000) _progressMeter.StepPercent = 10;
            //if (expectedByteCount < 100000) _progressMeter.StepPercent = 50;
            //long testMax = expectedByteCount / _progressMeter.StepPercent;
            //if (testMax < (long)9600000) _maxBufferSize = Convert.ToInt32(testMax); // otherwise keep it at 96000000
        }

       

        #endregion

        #region Methods

        // This internal method attempts to advance the buffer.
        private void AdvanceBuffer()
        {
            if (_isBufferLoaded == true)
            {
                // write the contents of the buffer
                _binaryWriter.Write(_buffer);

                // reposition the buffer after the last paste
                _bufferOffset = 0;  // file offset is tracked at the time of an individual write event
                //_fileOffset += _buffer.Length;
            }
            else
            {
                _isBufferLoaded = true;
            }
            
            // either way, dimension the next chunk
            _buffer = new byte[_maxBufferSize];

            // indicate where to start writing in the buffer
            _writeOffset = 0;
            
        }

        /// <summary>
        /// Finishes writing whatever is in memory to the file, closes the
        /// internal binary writer, the underlying file, clears the memory
        /// and disposes the filestream.
        /// </summary>
        public void Close()
        {
            if (_binaryWriter != null)
            {
              
                // Finish pasting any residual data to the file
                PasteBuffer();

                // Close the binary writer and underlying filestream
                _binaryWriter.Close();
            }
            _binaryWriter = null;
            _buffer = null;
            _progressMeter = null; // the IProgressHandler could be an undesired handle to a whole form or something
            if(_fileStream != null)_fileStream.Dispose();
            _fileStream = null;
        }

        /// <summary>
        /// Forces the buffer to paste all its existing values into the file, but does not
        /// advance the buffer, or in fact do anything to the buffer.  It does advance
        /// the position of the file index.
        /// </summary>
        private void PasteBuffer()
        {
            if (_writeOffset > -1)
            {
                _binaryWriter.Write(_buffer, 0, _writeOffset);
                _fileOffset += _writeOffset;
            }
        }
       

       

       

        

        #region Write Methods

        /// <summary>
        /// Reads a boolean form the buffer, automatcially loading the next buffer if necessary.
        /// </summary>
        /// <returns>A boolean value converted from bytes in the file.</returns>
        public void Write(bool value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Write(data);
        }

        /// <summary>
        /// Reads a character from two bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <param name="value">A character to write to the buffer, and eventually the file</param>
        public void Write(char value)
        {

            byte[] data = BitConverter.GetBytes(value);
            Write(data);

            
        }
        /// <summary>
        /// Reads an array of character from two bytes in the buffer, automatically loading
        /// </summary>
        /// <returns></returns>
        public void Write(char[] values)
        {
            List<byte> lstData = new List<byte>();
            foreach(byte b in values)
            {
                lstData.AddRange(BitConverter.GetBytes(b));
            }

            byte[] data = lstData.ToArray();
            Write(data);
        }

        /// <summary>
        /// Reads a double from the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <returns>A double value converted from bytes in the file.</returns>
        public void Write(double value)
        {
            Write(value, true);
        }


        /// <summary>
        /// Writes a double-precision floating point to 8 bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        /// <param name="value">A double-precision floating point decimal value to write as 8 bytes</param>
        public void Write(double value, bool isLittleEndian)
        {
        
            // Integers are 8 Bytes long.
            byte[] data = BitConverter.GetBytes(value);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }
            Write(data);
        }

        /// <summary>
        /// Writes the specified array of doubles to the file.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(double[] values)
        {
            byte[] rawbytes = new byte[values.Length * 8];
            System.Buffer.BlockCopy(values, 0, rawbytes, 0, rawbytes.Length);
            Write(rawbytes);
        }

        /// <summary>
        /// Writes the specified array of integers to the file.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(int[] values)
        {
            byte[] rawbytes = new byte[values.Length * 4];
            System.Buffer.BlockCopy(values, 0, rawbytes, 0, rawbytes.Length);
            Write(rawbytes);
        }


        /// <summary>
        /// By default, this will use little Endian ordering.
        /// </summary>
        /// <returns>An Int32 converted from the file.</returns>
        public void Write(int value)
        {
            Write(value, true);
        }

        /// <summary>
        /// Reads an integer from the file, using the isLittleEndian argument
        /// to decide whether to flip the bits or not.
        /// </summary>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        /// <param name="value">A 32-bit integer to write as 4 bytes in the buffer.</param>
        public virtual void Write(int value, bool isLittleEndian)
        {

            // Integers are 4 Bytes long.
            byte[] data = BitConverter.GetBytes(value);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }
            Write(data);
        }

        /// <summary>
        /// Writes an Int16 to the buffer.
        /// </summary>
        /// <param name="value">An Int16 to convert into 2 bytes to write to the buffer.</param>
        public virtual void Write(short value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Write(data);
        }
      

        /// <summary>
        /// Writes a single-precision floading point to 4 bytes in the buffer, automatically loading the next buffer if necessary.
        /// This assumes the value should be little endian.
        /// </summary>
        /// <param name="value">A Single to convert to 4 bytes to write to the buffer.</param>
        public virtual void Write(float value)
        {
            Write(value, true);
        }

        /// <summary>
        /// Reads a single-precision floading point from 4 bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <param name="value">A single-precision floating point converted from four bytes</param>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        public virtual void Write(float value, bool isLittleEndian)
        {
          

            // Integers are 4 Bytes long.
            byte[] data = BitConverter.GetBytes(value);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }

            Write(data);

        }

       
      
        /// <summary>
        /// Writes the specified bytes to the buffer, advancing the buffer automatically if necessary.
        /// </summary>
        /// <param name="value">An array of byte values to write to the buffer.</param>
        public virtual void Write(byte[] value)
        {
            bool finished = false;
            int bytesPasted = 0;
            int index = 0;
            int count = value.Length;
            if (_isBufferLoaded == false) AdvanceBuffer();
            do
            {
                int bytesInBuffer = _bufferSize - _writeOffset;
                if (count < bytesInBuffer)
                {
                    Array.Copy(value, index, _buffer, _writeOffset, count - bytesPasted);
                    _writeOffset += count - bytesPasted;
                    _fileOffset += count - bytesPasted;
                    finished = true;
                }
                else
                {
                    int sourceLeft = count - index;
                    if (sourceLeft > bytesInBuffer)
                    {
                        Array.Copy(value, index, _buffer, _writeOffset, bytesInBuffer);
                        index += bytesInBuffer;
                        bytesPasted += bytesInBuffer;
                        _fileOffset += bytesInBuffer;
                        _writeOffset += bytesInBuffer;
                        if (bytesPasted >= count)
                        {
                            finished = true;
                        }
                        else
                        {
                            AdvanceBuffer();
                        }
                    }
                    else
                    {
                        Array.Copy(value, index, _buffer, _writeOffset, sourceLeft);
                        index += sourceLeft;
                        bytesPasted += sourceLeft;
                        _fileOffset += sourceLeft;
                        _writeOffset += sourceLeft;
                        finished = true;
                    }

                }

            } while (finished == false);
            //_progressMeter.CurrentValue = _fileOffset;
            
        }

        
       


        #endregion

       
        /// <summary>
        /// This seeks both in the file AND in the buffer.  This is used to write only 
        /// desired portions of a buffer that is in memory to a file.
        /// </summary>
        /// <param name="offset">A 64 bit integer specifying where to skip to in the file.</param>
        /// <param name="origin">A System.IO.SeekOrigin enumeration specifying how to estimate the location.</param>
        public virtual long Seek(long offset, SeekOrigin origin)
        {
            long startPosition = 0;
            switch (origin)
            {
                case SeekOrigin.Begin: startPosition = offset;
                    break;
                case SeekOrigin.Current: startPosition = _writeOffset + offset;
                    break;
                case SeekOrigin.End: startPosition = _fileLength - offset;
                    break;
            }

            if (startPosition >= _fileLength || startPosition < 0)
            {
                // regardless of what direction, we need a start position inside the file
                throw new System.IO.EndOfStreamException(MessageStrings.EndOfFile);
            }
 
            // Only worry about resetting the buffer or repositioning the position
            // inside the buffer if a buffer is actually loaded.
            if (_isBufferLoaded)
            {

                long delta = startPosition - _fileOffset;

                if (delta > (long)(_bufferSize - _writeOffset))
                {
                    // The new position is beyond our current buffer
                    _buffer = null;
                    _writeOffset = -1;
                    _bufferOffset = -1;
                    _isBufferLoaded = false;
                }
                else
                {
                    // The new position is still inside the buffer
                    _writeOffset += Convert.ToInt32(delta);
                    _fileOffset = startPosition;
                    // we don't want to actually seek in the internal reader
                    return startPosition;
                }
            }
            // If no buffer is loaded, the file may not be open and may cause an exception when trying to seek.
            // probably better for tracking than not throwing one.

            _fileOffset = startPosition;
            if(_fileStream.CanSeek == true) _fileStream.Seek(offset, origin);
            return startPosition;
           
        }


        
       

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the actual array of bytes currently in the buffer
        /// </summary>
        public byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        /// <summary>
        /// Gets a long integer specifying the starting position of the currently loaded buffer
        /// relative to the start of the file.  A value of -1 indicates that no buffer is 
        /// currently loaded.
        /// </summary>
        public long BufferOffset
        {
            get { return _bufferOffset; }
            protected set { _bufferOffset = value; }
        }

        /// <summary>
        /// Gets an integer value specifying the size of the buffer currently loaded into memory.
        /// This will either be the MaxBufferSize, or a smaller buffer representing a smaller 
        /// remainder existing in the file.
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
            protected set { _bufferSize = value; }
        }

        /// <summary>
        /// Gets a boolean indicating whether there is currently any information loaded into the buffer.
        /// </summary>
        public virtual bool IsBufferLoaded
        {
            get { return _isBufferLoaded; }
            protected set { _isBufferLoaded = value; }
        }

        /// <summary>
        /// Gets the current read position in the file in bytes.
        /// </summary>
        public long FileOffset
        {
            get { return _fileOffset; }
            protected set { _fileOffset = value; }
        }

       

        /// <summary>
        /// Gets or sets the buffer size to read in chunks.  This does not
        /// describe the size of the actual 
        /// </summary>
        public virtual int MaxBufferSize
        {
            get { return _maxBufferSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(MessageStrings.ArgumentCannotBeNegative_S.Replace("%S", "BufferSize"));
                }
                _maxBufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the progress meter that is directly linked to the progress handler.
        /// </summary>
        protected virtual ProgressMeter ProgressMeter
        {
            get { return _progressMeter; }
            set { _progressMeter = value; }
        }

        /// <summary>
        /// Gets or sets the progress handler for this binary writer
        /// </summary>
        public virtual IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set
            {
                _progressHandler = value;
                _progressMeter = new ProgressMeter(_progressHandler);
            }
        }

        /// <summary>
        /// This acts like a placeholder on the buffer and indicates where reading will begin (relative to the start of the buffer)
        /// </summary>
        public virtual int WriteOffset
        {
            get { return _writeOffset; }
            protected set { _writeOffset = value; }
        }

        #endregion


       


    }
}
