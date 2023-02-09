using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{

    public class Parent
    {
        public string value = "parent";

        public void print()
        {
            Console.WriteLine("Parent");
        }
    }

    public class Child1 : Parent
    {
        public string value = "child1";
        public void print1()
        {
            Console.WriteLine("child1");
        }
    }

    public class Child2 : Parent
    {
        public new string value = "child2";
        public void print2()
        {
            Console.WriteLine("child2");
        }
    }

    public class TestClass
    {
        public Dictionary<string, object> properties;

        public TestClass()
        {
            properties = new Dictionary<string, object>();
            init();
        }

        private void init()
        {
            Parent parent = new Parent();
            Parent child1 = new Child1();
            Parent child2 = new Child2();

            properties.Add("parent", parent);
            properties.Add("child1", child1);
            properties.Add("child2", child2);
            




            print();
        }

        public void print()
        {
            //((Parent)properties["parent"]).print();
            //(properties["child1"]).print1();
            //(properties["child2"]).print2();
           

        }

    }



    class Class1
    {
        public Dictionary<string, object> properties;

        public Class1()
        {
            properties = new Dictionary<string, object>();
            init();
        }

        private void init()
        {
            float float1 = 10.1f;
            int int1 = 10;
            string string1 = "string";
            object bool1 = true;

            properties.Add("float", float1);
            properties.Add("int", int1);
            properties.Add("string", string1);
            properties.Add("bool", bool1);




            print();
        }

        private void print()
        {
            

            Console.WriteLine(properties["float"]);
            Console.WriteLine(properties["float"].GetType());
            if (properties["float"].GetType() == typeof(float))
            {
                Console.WriteLine("Floating point");
            }
            Console.WriteLine(properties["int"]);
            Console.WriteLine(properties["int"].GetType());
            Console.WriteLine(properties["string"]);
            Console.WriteLine(properties["string"].GetType());
            Console.WriteLine(properties["bool"]);
            Console.WriteLine(properties["bool"].GetType());
        }

        

        static void Main(string[] args)
        {
            Class1 c = new Class1();
        }


    }

}



