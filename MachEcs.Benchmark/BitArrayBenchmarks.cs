using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace MachEcs.Benchmark
{
    public class BitArrayBenchmarks
    {
        private BitArray _bitArray = new BitArray(64);
        private bool[] _boolArray = new bool[64];
        private long _longInt;

        [Benchmark]
        public void BitArrayBench()
        {
            if (_bitArray[30] == false)
            {
                _bitArray[15] = true;
            }
            else
            {
                _bitArray[15] = false;
            }
        }

        [Benchmark]
        public void BoolArrayBench()
        {
            if (_boolArray[30] == false)
            {
                _boolArray[15] = true;
            }
            else
            {
                _boolArray[15] = false;
            }
        }

        [Benchmark]
        public void LongIntBench()
        {
            if ((_longInt & (1 << 30)) != 0)
            {
                _longInt |= (1 << 15);
            }
            else
            {
                _longInt &= ~(1 << 15);
            }
        }
    }
}
