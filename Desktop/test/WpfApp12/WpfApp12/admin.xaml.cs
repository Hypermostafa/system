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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp12
{
    /// <summary>
    /// Interaction logic for admin.xaml
    /// </summary>
    public partial class admin : Page
    {

        taskmanagementsystemEntities2 db = new taskmanagementsystemEntities2();
        public admin()
        {
            InitializeComponent();
            setdata();
        }


        private void setdata()
        {
            datagrid.ItemsSource = db.TaskTables.Select(x => new
            {
                x.UserTable.UserName,
                x.TaskID,
                x.Title,
                x.Description,
                x.Status
            }).ToList();

           
            var status = db.TaskTables.Select(x => x.Status).Distinct().ToList();
            
            statuscombobox.ItemsSource = status;
        }
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(titlebox.Text) || string.IsNullOrWhiteSpace(descpritonbox.Text) || string.IsNullOrWhiteSpace(statuscombobox.SelectedItem?.ToString()))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                // Create new Task
                TaskTable newTask = new TaskTable
                {
                    Title = titlebox.Text,
                    Description = descpritonbox.Text,
                    Status = statuscombobox.SelectedItem.ToString(),
                    EmpID = int.TryParse(employeebox.Text, out int empID) ? empID : 0  // Safely parse Employee ID
                };

                db.TaskTables.Add(newTask);
                db.SaveChanges();

                // Refresh DataGrid
                setdata();

                MessageBox.Show("Task added successfully.");
            }
            catch (Exception ex)
            {
                // Handle any errors and display message
                MessageBox.Show($"Error adding task: {ex.Message}");

                // Display inner exception details if available
                if (ex.InnerException != null)
                {
                    MessageBox.Show($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

        // Update Task
        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (datagrid.SelectedItem == null)
                {
                    MessageBox.Show("Please select a task to update.");
                    return;
                }

                var selectedTask = datagrid.SelectedItem as dynamic;
                if (selectedTask == null)
                {
                    MessageBox.Show("Invalid task selected.");
                    return;
                }

                int taskId = selectedTask.TaskID;

                // Find the selected task from the database
                var taskToUpdate = db.TaskTables.FirstOrDefault(x => x.TaskID == taskId);
                if (taskToUpdate != null)
                {
                    taskToUpdate.Title = titlebox.Text;
                    taskToUpdate.Description = descpritonbox.Text;
                    taskToUpdate.Status = statuscombobox.SelectedItem.ToString();
                    taskToUpdate.EmpID = int.TryParse(employeebox.Text, out int empID) ? empID : 0;  // Safely parse Employee ID

                    db.SaveChanges();

                    // Refresh DataGrid
                    setdata();

                    MessageBox.Show("Task updated successfully.");
                }
                else
                {
                    MessageBox.Show("Task not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors and display message
                MessageBox.Show($"Error updating task: {ex.Message}");
            }
        }

        // Delete Task
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (datagrid.SelectedItem == null)
                {
                    MessageBox.Show("Please select a task to delete.");
                    return;
                }

                var selectedTask = datagrid.SelectedItem as dynamic;
                if (selectedTask == null)
                {
                    MessageBox.Show("Invalid task selected.");
                    return;
                }

                int taskId = selectedTask.TaskID;

                // Find the selected task from the database
                var taskToDelete = db.TaskTables.FirstOrDefault(x => x.TaskID == taskId);
                if (taskToDelete != null)
                {
                    db.TaskTables.Remove(taskToDelete);
                    db.SaveChanges();

                    // Refresh DataGrid
                    setdata();

                    MessageBox.Show("Task deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Task not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors and display message
                MessageBox.Show($"Error deleting task: {ex.Message}");
            }
        }
    }
}
