using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NVentimiglia.Tests
{
    public class Tests
    {

        protected static int counter;

        #region helpers
        public struct MyObject : IBitModel, IEquatable<MyObject>
        {
            public int x;
            public int y;

            public MyObject[] children;

            public void Parse(BitSerializer stream)
            {
                stream.Parse(ref x);
                stream.Parse(ref y);
                stream.Parse(ref children);
            }

            public MyObject Copy()
            {
                return new MyObject()
                {
                    x = x,
                    y = y,
                    children = children,
                };
            }

            public static MyObject Create()
            {
                var model = new MyObject()
                {
                    x = ++counter,
                    y = ++counter,
                    children = new MyObject[0]
                };
                return model;
            }

            public bool Equals(MyObject obj)
            {
                return x == obj.x && y == obj.y;
            }

            public override int GetHashCode()
            {
                return (x & y);
            }
        }

        void AssertArray<T>(T[] a, T[] b) where T : IEquatable<MyObject>
        {
            Assert.AreEqual(a.Length, b.Length);
            var comp = EqualityComparer<T>.Default;
            for (int i = 0; i < a.Length; i++)
            {
                Assert.IsTrue(comp.Equals(a[i], b[i]));
            }
        }
        void AssertArray(int[] a, int[] b)
        {
            Assert.AreEqual(a.Length, b.Length);
            for (int i = 0; i < a.Length; i++)
            {
                Assert.IsTrue(a[i].Equals(b[i]));
            }
        }
        #endregion

        [Test]
        public void TestInt3()
        {
            var stream = new BitSerializer(); stream.IsWriting = true;
            for (int i = 0; i < 1000000; i++)
            {
                var data = 69;
                var val = 0;

                stream.Reset();
                stream.Parse(ref data);

                stream.Reset();
                stream.IsWriting = false;

                stream.Parse(ref val);

                Assert.AreEqual(val, data);
            }
        }

        [Test]
        public void TestIntArray()
        {
            var data = new[] { 6, 6, 6 };
            int[] val = null;
            var stream = new BitSerializer(); stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);

            AssertArray(val, data);
        }

        [Test]
        public void TestFloat()
        {
            float data = 69f;
            float val = 69f;
            var stream = new BitSerializer(); stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);

            Assert.AreEqual(val, data);
        }


        [Test]
        public void TestFloatArray()
        {
            float[] data = new[] { 6f, 9f, -3 };
            float[] val = null;
            var stream = new BitSerializer();
            stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);

            Assert.IsTrue(val.Length.Equals(data.Length));

            for (int i = 0; i < data.Length; i++)
            {
                Assert.IsTrue(data[i].Equals(val[i]));
            }
        }

        [Test]
        public void TestString()
        {
            string data = "A Char is 2 bytes";
            string val = null;
            var stream = new BitSerializer(); stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);

            Assert.IsTrue(val.Equals(data));
        }

        [Test]
        public void TestObject()
        {
            var data = MyObject.Create();
            MyObject val = default(MyObject);

            var stream = new BitSerializer();
            stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);

            var x = val.x;
            var xa = data.x;
            var y = val.y;
            var ya = data.y;

            Console.WriteLine(x + " " + xa);
            Console.WriteLine(y + " " + ya);


            Assert.IsTrue(val.Equals(data));
        }

        [Test]
        public void TestObjectArray()
        {
            MyObject[] data = new[] { MyObject.Create(), MyObject.Create(), MyObject.Create() };
            MyObject[] val = null;

            var stream = new BitSerializer(); stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);

            AssertArray(val, data);
        }

        [Test]
        public void TestNestedObject()
        {
            MyObject data = MyObject.Create();
            data.children = new[] { MyObject.Create(), MyObject.Create(), MyObject.Create() };

            MyObject val = default(MyObject);

            var stream = new BitSerializer();
            stream.IsWriting = true;
            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);

            Assert.IsTrue(val.Equals(data));
            AssertArray(val.children, data.children);
        }

        [Test]
        public void TestParseObject()
        {
            MyObject data = MyObject.Create();
            data.children = new[] { MyObject.Create(), MyObject.Create(), MyObject.Create() };
            MyObject val = default(MyObject);

            var stream = new BitSerializer(); stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);


            Assert.IsTrue(val.Equals(data));
            AssertArray(val.children, data.children);
        }

        [Test]
        public void TestNativeArray()
        {
            MyObject data = MyObject.Create();
            data.children = new[] { MyObject.Create(), MyObject.Create(), MyObject.Create() };
            MyObject val = default(MyObject);

            var stream = new BitSerializer(); stream.IsWriting = true;

            stream.Parse(ref data);
            stream.Reset();
            stream.IsWriting = false;
            stream.Parse(ref val);


            Assert.IsTrue(val.Equals(data));
            AssertArray(val.children, data.children);
        }
    }
}
