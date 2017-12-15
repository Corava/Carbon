using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Carbon {
    /*
  * Note Area: 
  * Create ways to create Posts, ContentAreas, Groups, etc.
  * xudo = god, create him
  * params string[]
  * Create a global user for whenever someone logs in
  * 
  * 
  */
    //var indexOfUser = members.FirstOrDefault(o => o.UID == user.UID);
    //Verify you wish to remove user

    #region Graphical Classes
    /* Simple Class for Pictures, stores location on drive
     * 
     * 
     */
    public class LinkedPictures {
        private List<String> paths;
    }

    /* Class containing the data included with a Post
     * 
     * External Calls:
     * DateTime
     * PostHistory
     * User
     * ContentArea
     */

    public class Post {
        public string postContent { get; set; }
        public string postTitle { get; set; }
        public List<User> users { get; set; }
        public DateTime date { get; set; }
        //public PostHistory postHistory;
        public ContentArea contentArea { get; set; }
        public char SortableIdentifier { get; set; }
        public bool editable { get; set; } = true;

        public Post(string title, List<User> members, ContentArea conAra) {
            postTitle = title;
            users = members;
            contentArea = conAra;
        }

        public void Set_Identifier(char SI) {
            SortableIdentifier = SI;
        }

        public char Get_Identifier() {
            return SortableIdentifier;
        }

        public ContentArea Get_ContentArea() {
            return contentArea;
        }

        public string Get_Title() {
            return postTitle;
        }
    }

    /* Class containing a collection 
     * 
     */
    public class Meeting {
        public string meetingName { get; set; }
        public List<ContentArea> contentAreas { get; set; }
        public List<Post> posts { get; set; } = new List<Post>();
        public List<Post> sortedPosts { get; set; } = new List<Post>();
        public List<User> meetingUsers { get; set; }
        public Formatting parentGroupFormatting { get; set; }

        public Meeting(string nameOfMeeting, List<User> users, List<ContentArea> areasOfContent, Formatting gf) {
            meetingName = nameOfMeeting;
            meetingUsers = users;
            contentAreas = areasOfContent;
            parentGroupFormatting = gf;
        }

        public void Add_Post(string title, List<User> members, ContentArea conAra) {
            posts.Add(new Post(title, members, conAra));
        }

        public void Set_Identifiers() {
            var order = parentGroupFormatting.outputStringList().OrderBy(x => x).ToList<string>();
            var orderDict = new Dictionary<string, char>();
            for (int i = 0; i < order.Count; i++) {
                orderDict.Add(order[i], (char)(i + 65));
            }
            foreach (Post p in posts) {
                p.Set_Identifier(orderDict[p.Get_ContentArea().contentType]);
            }
        }

        public void Remove_Post(Post postToRemove) {
            posts.Remove(postToRemove);
        }

        public void Construct_Print() {
            Set_Identifiers();
            sortedPosts = posts.OrderBy(x => x.Get_Identifier()).ToList();
        }

        public List<Post> Get_Print() {
            Construct_Print();
            return sortedPosts;
        }
    }

    /* Class containing a type for a post
     * 
     * External Calls:
     * 
     */
    public class ContentArea {
        private string _contentType;
        public string contentType {
            get { return _contentType; }
        }

        public ContentArea(String ct) {
            _contentType = ct;
        }
    }

    /* Class containing a collection of meeetings and group specific parameters
     * 
     * External Calls
     * Meetings
     * User
     * GroupFormatting
     * ContentArea
     */
    public class Group {
        public List<Meeting> meetings { get; set; } = new List<Meeting>();
        public List<User> groupMembers { get; set; }
        public Formatting groupFormatting { get; set; }
        public List<ContentArea> contentAreas { get; set; }

        public Group() {
            //verify that someone has the rights to change
            //interesting constructor things here
        }

        public void Add_User(User user) {
            groupMembers.Add(user);
        }

        public void Remove_User(User user) {
            groupMembers.Remove(user);
        }

        public void Add_Post(Meeting whatMeeting, string title, List<User> members, ContentArea conAra) {
            whatMeeting.Add_Post(title, members, conAra);
        }

        public void Remove_Post(Meeting whatMeeting, Post whatPost) {
            whatMeeting.Remove_Post(whatPost);
        }

        public void Add_Meeting(string nameOfMeeting, List<User> users, List<ContentArea> areasOfContent) {
            meetings.Add(new Carbon.Meeting(nameOfMeeting, users, areasOfContent, groupFormatting));
        }

        public void Remove_Meeting(Meeting whatMeeting) {
            meetings.Remove(whatMeeting);
        }
        //GroupFormatting.orderofContentAreas
    }
    #endregion

    #region Configuration Classes
    /* Class containing formatting rules for post organization and order
     * 
     * External Calls
     * ContentArea
     * 
     */
    public class Formatting {
        private List<ContentArea> orderOfContentAreas = new List<ContentArea>();

        public Formatting(params ContentArea[] hi) {
            foreach (ContentArea ca in hi) {
                orderOfContentAreas.Add(ca);
            }
        }

        public List<string> outputStringList() {
            List<string> tmpA = new List<string>();
            foreach (ContentArea ca in orderOfContentAreas) {
                tmpA.Add(ca.contentType);
            }
            return tmpA;
        }

        public void Add_ContentArea(ContentArea ca) {
            orderOfContentAreas.Add(ca);
        }

        public void Remove_ContentArea(int index) {
            orderOfContentAreas.RemoveAt(index);
        }
    }
    #endregion

    #region History Classes
    /* Class containing every iteration of strings that a user has inputed
     * 
     * External Calls:
     * User
     */
    public class PostHistory {
        private List<int> change_order;
        private List<User> change_user;
        private List<string> change_text;
        private List<string> text_history;
    }
    #endregion

    #region User Classes
    /* Class for identifying users of the software
     * 
     * External Calls:
     * UserAccess
     * 
     * Possible Changes:
     * Integrating with Microsoft AD services
     */
    public class User {
        private UserAccess userRights;
        private Formatting userFormatting;
        private string name;
        private string origin;
        private string extraInfo;
        private string _UID;
        public string UID {
            get { return _UID; }
        }

        public User(string name, string origin, string extraInfo = "") {
            this.name = name;
            this.origin = origin;
            this.extraInfo = extraInfo;
            //code to generate or integrate user ID
        }

    }
    #endregion

    #region Permissions
    enum Levels { NA, User, GroupAdmin, OriginAdmin, xudo }
    /* Class for assigning roles for users
     * 
     * External Calls:
     * N/A
     */
    public class UserAccess {
        protected bool editPost;
        protected bool createPost;
        protected bool deletePost;
        protected bool createGroupandMeeting;
        protected bool editGroupandMeeting;
        protected bool removeGroup;
    }

    /* Class for specifying user access by group or by community
     * 
     * External Calls:
     * N/A
     * 
     * Implements:
     * UserAccess
     */
    public class SpecificUserAccess : UserAccess {
        private string groupById;
        private string communityById;
    }


    //How to Trigger authentication





    #endregion
}