using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GradesPrototype.Data;
using GradesPrototype.Services;

namespace GradesPrototype.Controls
{
    /// <summary>
    /// Interaction logic for AssignStudentDialog.xaml
    /// </summary>
    public partial class AssignStudentDialog : Window
    {
        public AssignStudentDialog()
        {
            InitializeComponent();
        }

        // TODO: Exercise 4: Task 3b: Refresh the display of unassigned students
        private void Refresh()
        {
            List<int> unassigned = new List<int>();

            var unassignedStudents = from u in DataSource.Students where u.StudentID == 0 select u;
            if (unassignedStudents.Count() != 0)
            {
                txtMessage.Visibility = Visibility.Collapsed;
                list.Visibility = Visibility.Visible;
                list.ItemsSource = unassignedStudents;
            }
            else
            {
                list.Visibility = Visibility.Collapsed;
                txtMessage.Visibility = Visibility.Visible;
            }

        }

        private void AssignStudentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        // TODO: Exercise 4: Task 3a: Enroll a student in the teacher's class
        private void Student_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button studentClicked = sender as Button; // Not sure what the flow of the program is here...

                int studentID = (int)studentClicked.Tag; //How does the compiler know how to get the StudentID instead of the (if assigned) TeacherID? Does it select the first integer it can find?

                Student student = (from s in DataSource.Students where studentID == s.StudentID select s).First();

                string promptMessage = string.Format("Do you wish to add {0} {1} to your class?", student.FirstName, student.LastName);
                MessageBoxResult prompt = MessageBox.Show(promptMessage, "Confirm?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (prompt == MessageBoxResult.Yes)
                {
                    //int teachnum = SessionContext.CurrentTeacher.TeacherID; //This isnt neccessary?
                    SessionContext.CurrentTeacher.EnrollInClass(student);
                }
            }
            catch
            {
                throw new Exception("Something went wrong");
            }



        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog box
            this.Close();
        }
    }
}
