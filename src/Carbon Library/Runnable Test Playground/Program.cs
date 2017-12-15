using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Carbon;

class Program {
    static void Main(string[] args) {
        var userGroup = new List<User>();
        userGroup.Add(new Carbon.User("Stefan", "School"));
        userGroup.Add(new Carbon.User("Stefant", "Schoolt"));
        var cal = new List<ContentArea>();
        cal.Add(new ContentArea("Stuff"));
        cal.Add(new ContentArea("Things"));
        Group testGroup = new Carbon.Group();
        testGroup.meetings.Add(new Meeting("testMeeting", userGroup, cal, new Formatting(new ContentArea("Stuff"), new ContentArea("Things"))));
        testGroup.meetings[0].Add_Post("Test Post1", userGroup, new ContentArea("Stuff"));
        testGroup.meetings[0].Add_Post("Test Post2", userGroup, new ContentArea("Things"));
        testGroup.meetings[0].Add_Post("Test Post3", userGroup, new ContentArea("Things"));
        testGroup.meetings[0].Add_Post("Test Post4", userGroup, new ContentArea("Stuff"));
        var output = testGroup.meetings[0].Get_Print();
        foreach (Post a in output) {
            Console.WriteLine(a.Get_Title());
        }
        Console.ReadKey();
    }
}

/*

    
    
var order = gf.outputStringList().GroupBy(x => x);
finalSortOrder = new List<string>();
            foreach (var sortitem in order) {
                foreach (string sort in sortitem) {
                    finalSortOrder.Add(sort);
                }
            }




    */