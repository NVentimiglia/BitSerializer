# BitSerializer
- Minimal binary serializer designed for performance.
- Optimized for games / high performance apps.
- Minimal header, structured layout, unsafe binary serialization.
- Supports primiatives, arrays, and objects using an interface.
- IBitModel interface allows for object serialization without any reflection
- One class, One interface.


### Serialization
    var stream = new BitSerializer(); 
    stream.IsWriting = true;
    stream.Reset();
    
    int data = 999;
    stream.Parse(ref data);
    
    IBitModel myObject;
    stream.Parse(ref myObject);
    
  
### Deserialization
    var stream = new BitSerializer(); 
    stream.IsWriting = false;
    stream.Reset();
    
    int data = 999;
    stream.Parse(ref data);
    
    IBitModel myObject;
    stream.Parse(ref myObject);
    
      
### IBitModel
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
    }

### Polmorphism
- For polymorphism, I write a enum / const which maps a type to a byte. 
- In my writer, I will preface my serailization with a header byte to identify the type.
- In my reader, I will read the byte and use a switch statement to deserialize the right type.
