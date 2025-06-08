# Library BsonData (C#)

- Create new way to store and manage BSON data in your application. 
- Using NoSQL database principles to handle data efficiently.
- Supports CRUD operations, indexing, and querying.

---

## Content

- [Overview](#Overview)
- [Requirement](#Requirement)
- [Install](#Install)
- [How to use](#How-to-use)
- [Feature](#Feature)
- [Example](#Example)
- [Contribute](#Contribute)
- [License](#License)

---

## Overview

This library provides a simple and efficient way to work with BSON data structures, allowing developers to easily store, retrieve, and manipulate data in a NoSQL style.
See some example: https://github.com/TimDeveloper97/vst-tram-thuy-van/blob/dev/db/BsonDataExample/BsonDataExample/Program.cs

## Requirement

- Develop a library: .NET Framework 4.5+ or .NET Core 2.0+
- Visual Studio 2017+ or any compatible IDE

## Install

Install References: Add Reference -> Browse -> Find `<NameLibrary>` -> Select Dll file.

or add project file `.csproj`:
```xml
<PackageReference Include="<NameLibrary>" Version="x.x.x" />
```

## How to use

Example code to quickly start using the library:

```csharp
using BsonData;

public partial class DB
{
    static public MainDatabase Main { get; private set; }
    static public void Register(string path)
    {
        Main = new MainDatabase("MainDB");
        Main.Connect(path);
    }

    static Collection seq;
    static public Collection SEQ
    {
        get
        {
            if (seq == null)
            {
                seq = Main.GetCollection(nameof(SEQ));
            }
            return seq;
        }
    }

}

DB.Register(Environment.CurrentDirectory);
var doc = new Document();
foreach (var key in keys)
{
    Console.Write($"{key}: ");
    doc.Push(key, Console.ReadLine());
}
doc.Time = DateTime.Now;

DB.SEQ.Insert(doc);
```

## Feature

| Feature           | Description                                 |
|-------------------|---------------------------------------------|
| MainDatabase      | Database                                    |
| Collection        | Collection exist in Database                |
| Document          | Document exist in Collection                |
| Connect           | Create directory to store database          |
| GetCollection     | Get collection by name                      |
| Insert            | Insert document in collection               |
| Find              | Find document id in collection              |
| Select            | Select all document in collection           |
| Delete            | Delete document id in collection            |
| ...               | ...                                         |

### Feature

#### 1. CRUD MainDatabase : Database

- **Purpose:**  
  Class `MainDatabase` is used to manage the database connection and operations.
- **Use:**  

| Feature           | Description                                    |
|-------------------|------------------------------------------------|
| Add               | Add more database to Main database in "/SUBS"  |
| Remove            | Remove child database in "/SUBS"               |
| StartStorageThread| Store after minisecond                         |
| Connect           | Create directory to store database             |
| Disconnect        | Disconnect thread to database                  |
| ...               | ...                                            |

#### 2. CRUD Collection

- **Purpose:**  
  Class `Collection` is used to manage the collect in database.
- **Use:**  

| Feature           | Description                                    |
|-------------------|------------------------------------------------|
| Database          | Database get information only                  |
| Name              | Name of database                               |
| Count             | Number collection in database (có tính cả database trong child không hay add vào)???  |
| Insert            | Insert document in collection                  |
| Find              | Find document id in collection                 |
| Select            | Select all document in collection              |
| Delete            | Delete document id in collection               |
| InsertOrUpdate    | Insert or update document id in collection     |
| DistinctRow       | Get document by column name in collection      |
| DistinctColumn    | Error???            |
| GroupBy           | Group document same value with input column name in collection            |
| Each              | Foreach document id in collection              |
| FindAndDelete     | Find and delete document id in collection      |
| FindAndUpdate     | Find and update document id in collection      |
| GetDocument    | Get document by name in collection      |
| SelectContext    | Get document by name in collection      |
| ...               | ...                                            |

#### 3. CRUD Document

- **Purpose:**  
  Class `Document` is used to manage the collect in database.
- **Use:**

Every document exist 3 type: string, object, array.

| Feature           | Description                                    |
|-------------------|------------------------------------------------|
| Push          | Push key and value to document                  |
| Pop              | Pop key in document                               |
| GetArray             | Get array object in document  |
| GetString            | Get string in document                  |
| Add              | Add object in document                 |
| ContainsKey            | Contains key in document              |
| ContainsValue            | Contains value in document               |
| InsertOrUpdate    | Insert or update document id in collection     |
| DistinctRow       | Get document by column name in collection      |
| DistinctColumn    | Error???            |
| GroupBy           | Group document same value with input column name in collection            |
| Each              | Foreach document id in collection              |
| FindAndDelete     | Find and delete document id in collection      |
| FindAndUpdate     | Find and update document id in collection      |
| ...               | ...                                            |

## Example

Example to run simple console application using BsonData library:

```csharp
using BsonData;
using System;
using System.Linq;

namespace System
{
    partial class Document
    {
        public string MTI { get => GetString(nameof(MTI)); set => Push(nameof(MTI), value); }
        public string MHU { get => GetString(nameof(MHU)); set => Push(nameof(MHU), value); }
        public DateTime? Time { get => GetDateTime(nameof(Time)); set => Push(nameof(Time), value); }
    }

    public partial class DB
    {
        static public MainDatabase Main { get; private set; }
        static public void Register(string path)
        {
            Main = new MainDatabase("MainDB");
            Main.Connect(path);
        }

        static Collection seq;
        static public Collection SEQ
        {
            get
            {
                if (seq == null)
                {
                    seq = Main.GetCollection(nameof(SEQ));
                }
                return seq;
            }
        }

    }
}

namespace System
{
    public abstract class View
    {
        public View Run()
        {
            Console.Clear();
            return CreateView();
        }

        public void Stop()
        {
            Console.Write("Press any key ... ");
            Console.ReadKey();
        }

        public abstract View CreateView();
    }

    public class Menu : View
    {
        public override View CreateView()
        {
            var ops = new string[] {
                "Add record",
                "List",
                "Find record",
            };

            Console.WriteLine("**** MENU ****");

            int i = 0;
            foreach (var s in ops)
            {
                Console.WriteLine($"{++i} {s}");
            }

            while (true)
            {
                Console.Write(">> ");
                var cmd = Console.ReadLine();
                switch (cmd[0])
                {
                    case '1': return new Editor();
                    case '2': return new Report();
                    case '3': return new Find();

                    default:
                        Console.WriteLine("Operation Invalid");
                        break;
                }
            }
        }
    }
    public class Report : View
    {
        public override View CreateView()
        {
            var lst = DB.SEQ.Select().OrderByDescending(x => x.Time);
            int i = 0;
            foreach (var e in lst)
            {
                Console.WriteLine(e.Join("\t", "_id", "Time", "MTI", "MHU"));
                if (++i == 10)
                {
                    Stop();
                    i = 0;
                }
            }
            Stop();
            return null;
        }
    }
    public class Editor : View
    {
        public override View CreateView()
        {
            var keys = new string[] {
                "MTI", "MHU",
            };
            Console.WriteLine("**** Create Record ****");

            var doc = new Document();
            foreach (var key in keys)
            {
                Console.Write($"{key}: ");
                doc.Push(key, Console.ReadLine());
            }
            doc.Time = DateTime.Now;

            DB.SEQ.Insert(doc);
            return null;
        }
    }
    public class Find : View
    {
        public override View CreateView()
        {
            var keys = new string[] {
                "MTI", "MHU",
            };
            Console.WriteLine("**** Find Record ****");

            var doc = new Document();
            foreach (var key in keys)
            {
                Console.Write($"{key}: ");
                doc.Push(key, Console.ReadLine());
            }

            DB.SEQ.Find("4c141101c46c16011aa18a58", (d) =>
            {
                Console.WriteLine(d);
            });
            return null;
        }
    }
}

namespace BsonDataExample
{
    class Program
    {
        static void Main(string[] args)
        {
            DB.Register(Environment.CurrentDirectory);

            var menu = new Menu();
            View view = menu;
            while (true)
            {
                view = view.Run() ?? menu;
            }
        }
    }
}

```

## Contribute

- Vu Song Tung (Technical Leader)
- https://github.com/TimDeveloper97
- Contract: 0394852798 - Dinh Duy Anh (Developer)

## License

License (MIT).