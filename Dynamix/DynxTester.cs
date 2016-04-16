namespace IROM.Dynamix
{
	using System;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Diagnostics;
	
	/// <summary>
	/// Performs tests on Dynx to ensure robustness.
	/// </summary>
	public static class DynxTester
	{
		public static void Main()
		{
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
			Exception ex = null;
			try
			{
				const int range = 2;
				
				//# Dynx Test 1
				{
					for(int i = -range; i <= range; i++)
					{
						Dynx<int> d1 = new Dynx<int>();
						d1.Value = i;
						if(d1.Value != i) throw new Exception();
					}
				}
				
				//# Dynx Test 2
				{
					for(int i = -range; i <= range; i++)
					for(int j = -range; j <= range; j++)
					{
						Dynx<int> d1 = new Dynx<int>();
						Dynx<int> d2 = new Dynx<int>();
						d1.Value = i;
						d2.Exp = () => d1.Value + j;
						if(d2.Value != (i + j)) throw new Exception();
					}
				}
				
				//# Dynx Test 3
				{
					for(int i = -range; i <= range; i++)
					for(int j = -range; j <= range; j++)
					for(int k = -range; k <= range; k++)
					{
						Dynx<int> d1 = new Dynx<int>();
						Dynx<int> d2 = new Dynx<int>();
						d1.Value = i;
						d2.Exp = () => d1.Value + j;
						d1.Value = k;
						if(d2.Value != (k + j)) throw new Exception();
					}
				}
				
				//# Dynx Test 4
				{
					for(int i = -range; i <= range; i++)
					for(int j = -range; j <= range; j++)
					for(int k = -range; k <= range; k++)
					for(int l = -range; l <= range; l++)
					{
						Dynx<int> d1 = new Dynx<int>();
						Dynx<int> d2 = new Dynx<int>();
						Dynx<int> d3 = new Dynx<int>();
						d1.Value = i;
						d2.Exp = () => d1.Value + j;
						d3.Exp = () => d2.Value + k;
						d1.Value = l;
						if(d3.Value != (l + k + j)) throw new Exception();
					}
				}
				
				//# Dynx Test 5
				{
					for(int i = -range; i <= range; i++)
					for(int j = -range; j <= range; j++)
					for(int k = -range; k <= range; k++)
					for(int l = -range; l <= range; l++)
					{
						Dynx<int> d1 = new Dynx<int>();
						Dynx<int> d2 = new Dynx<int>();
						Dynx<int> d3 = new Dynx<int>();
						d1.Value = i;
						d2.Exp = () => d1.Value + j;
						d3.Exp = () => d2.Value + k;
						d1.Value = l;
						d2.Value = i;
						if(d3.Value != (i + k)) throw new Exception();
					}
				}
				
				//# Dynx Test 6
				{
					object dummy = new object();
					GCHandle h1 = GCHandle.Alloc(dummy, GCHandleType.Weak);
					dummy = null;
					GC.Collect();
					//only test in Release mode with no debugger. Thus, check with dummy object first.
					if(h1.Target == null)
					{
						Dynx<int> d1 = new Dynx<int>();
						Dynx<int> d2 = new Dynx<int>();
						d1.Value = 0;
						d2.Exp = () => d1.Value + 1;
						GCHandle h2 = GCHandle.Alloc(d2, GCHandleType.Weak);
						d2 = null;
						GC.Collect();
						if(h2.Target != null) Console.WriteLine("Warning: Test 6 Failed. Target value: " + (h2.Target as Dynx<int>).Value);
					}else
					{
						Console.WriteLine("Warning: Debug mode or debugger active. Test 6 cannot be performed.");
					}
				}
				
				//# Dynx Test 7
				{
					const int size = 1000000;
					const int changes = 10;
						
					Random rand = new Random(0);
					int[] offsets = new int[size];
					for(int i = 0; i < size; i++)
						offsets[i] = rand.Next(4);
					int[] offsets2 = new int[changes];
					for(int i = 0; i < changes; i++)
						offsets2[i] = rand.Next(100);
					Dynx<int>[] tests = new Dynx<int>[size];
					Stopwatch watch = new Stopwatch();
					
					GC.Collect();
					long mem = GC.GetTotalMemory(true);
					
					Console.WriteLine("Test 7: Part 1 Start");
					watch.Start();
					
					tests[0] = new Dynx<int>(0);
					for(int i = 1; i < size; i++)
					{
						int local = i - 1;//Math.Min(i, offsets[i] + 1);
						tests[i] = new Dynx<int>();
						tests[i].Exp = () => tests[local].Value + 1;
					}
					
					watch.Stop();
					Console.WriteLine("Test 7: " + watch.ElapsedMilliseconds + "ms");
					
					Console.WriteLine("Test 7: Part 2 Start");
					watch.Restart();
					for(int i = 0; i < changes; i++)
					{
						tests[0].Value = offsets2[i];
						if(tests[tests.Length-1].Value != (offsets2[i] + tests.Length - 1)) throw new Exception("Dynx answer incorrect: " + tests[tests.Length-1].Value + ", expected: " + offsets2[i]);
					}
					
					watch.Stop();
					Console.WriteLine("Test 7: " + watch.ElapsedMilliseconds + "ms");
					Console.WriteLine("Test 7: " + ((changes * size) / watch.ElapsedMilliseconds) + " updates/ms");
					
					GC.Collect();
					mem = GC.GetTotalMemory(true) - mem;
					
					Console.WriteLine("Test 7: Word Memsize: " + (((mem / size) - 4) / IntPtr.Size));
				}
			}catch(Exception e)
			{
				ex = e;
			}
			if(ex != null)
			{
				Console.WriteLine(ex);
			}else
			{
				Console.WriteLine("Success");
			}
			Console.ReadKey();
		}
	}
}
