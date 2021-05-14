# BitSerializer
- Minimal binary serializer designed for performance.
- Optimized for games / high performance apps.
- Uses a structured layout, minimal headers, no box cast, and no reflection for maximum speed.
- Supports primitives, arrays, and objects using the IBitModel interface.
- One class, One interface.


### Serialization
    var stream = new BitSerializer(); 
    stream.IsWriting = true;
    stream.Reset();
    
    int data = 999;
    stream.Parse(ref data);
    data = stream.Parse(data);
    
    IBitModel myObject;
    stream.Parse(ref myObject);
    myObject = stream.Parse(myObject);

    _socket.PostBytes(stream.Data, stream.Index);
    
  
### Deserialization
    var stream = new BitSerializer(); 
    stream.Data = _socket.GetBytes();
    stream.IsWriting = false;
    stream.Reset();
    
    int data = 999;
    stream.Parse(ref data);
    data = stream.Parse(data);
    
    IBitModel myObject;
    stream.Parse(ref myObject);
    myObject = stream.Parse(myObject);
    
      
### IBitModel
    public struct MyObject : IBitModel
    {
        public int x;
        public int y { get; set; }

        public MyObject[] children;

        public void Parse(BitSerializer stream)
        {
            stream.Parse(ref x);
            y = stream.Parse(y);
            stream.Parse(ref children);
        }
    }

### Polymorphism
- For polymorphism, I write a enum / const which maps a type to a byte. 
- In my writer, I will preface my serialization with a header byte to identify the type.
- In my reader, I will read the byte and use a switch statement to deserialize the right type.
