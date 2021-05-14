# BitSerializer
- Minimal binary serializer designed for performance.
- Optimized for games / high performance apps.
- Uses a structured layout, minimal headers, no box cast, and no reflection for maximum speed.
- Supports primitives, arrays, and objects using the IBitModel interface.
- One class, One interface.

### Implementation Strategy
- Use one method 'Parse' and recursion to eliminate superfluous header data and separate read/write methods.

### Serialization
    var stream = new BitSerializer(); 
    stream.IsWriting = true;
    stream.Reset();
    
    int data = 999;
    IBitModel myObject;

    // This writes 'data' and 'myObject' into the internal buffer
    stream.Parse(ref data);
    stream.Parse(ref myObject);

    _socket.PostBytes(stream.Data, stream.Index);
    
  
### Deserialization
    var stream = new BitSerializer(); 
    stream.Data = _socket.GetBytes();
    stream.IsWriting = false;
    stream.Reset();
    
    int data = 999;
    IBitModel myObject;

    // This reads 'data' and 'myObject' from the internal buffer
    stream.Parse(ref data);
    stream.Parse(ref myObject);    

### Helpers
    var stream = new BitSerializer(); 
    
    IBitModel model = stream.ReadFrom<IBitModel>(byte[] buffer);
    IBitModel model = stream.Read<IBitModel>();

    stream.WriteTo<IBitModel>(model, byte[] buffer);
    stream.Write<IBitModel>(model);    
    byte[] copy = stream.WriteCopy<IBitModel>(model);
      
### IBitModel
    public struct MyObject : IBitModel
    {
        public int x;
        public int y;

        public MyObject[] children;

        public void Parse(BitSerializer stream)
        {
            // One method for mapping objects
            // Uses recursion to eliminate headers
            stream.Parse(ref x);
            stream.Parse(ref y);
            stream.Parse(ref children);
        }
    }

### Polymorphism
- For polymorphism, I write a enum / const which maps a type to a byte. 
- In my writer, I will preface my serialization with a header byte to identify the type.
- In my reader, I will read the byte and use a switch statement to deserialize the right type.
