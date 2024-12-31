
using System;
using System.Data;
using System.Dynamic;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

namespace HabitTracker

{
    class Program
    {
        //int rowTotal = 0;
        static void Main(string[] args)
        {
            string curFile = @"C:\Users\ofarr\OneDrive\Coding\HabitTrackerApp\HabitTracker.db";

            //Console.Clear();
            //if (data source HabitTracker.db exists, then don't use intializedb method
            if (!File.Exists(curFile))
            {
                initializeDatabase();
            }
            Console.WriteLine(System.Globalization.CultureInfo.CurrentCulture);
            Console.WriteLine("Habit Tracker v1.0");
            trackerMenu();
            Console.WriteLine("Closing Tracker");
        }

        static void initializeDatabase()
        {
            //connection to sqlite database
            using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                //initialize db with id, Habit, Quantity, & Date
                command.CommandText =
                @"
                    CREATE TABLE TrackedHabits (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        Habit TEXT,
                        Quantity INTEGER,
                        Date TEXT
                    );
                ";
                //Date Format TEXT as ISO8601 strings ("YYYY-MM-DD HH:MM:SS.SSS").
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }

        }

        static void trackerMenu()
        {
            string choice = "";


            //switch loop that has insert, update, delete, and exit action.
            do
            {

                Console.WriteLine("Make a selection by number (1-4) below, or type 'exit'");
                Console.WriteLine("1. Insert Habit \n2. Update Habit \n3. Delete Habit \n4. Show Tracked Habits \n'exit'");
                bool validEntry = false;

                string? inputChoice = "";
                do
                {
                    inputChoice = Console.ReadLine();
                    {
                        if (inputChoice != null && (inputChoice == "1" || inputChoice == "2" || inputChoice == "3" || inputChoice == "4" || inputChoice == "exit"))
                        {
                            //if (inputChoice == "1" || inputChoice == "2" || inputChoice == "3" || inputChoice == "4")
                            choice = inputChoice;
                            validEntry = true;
                        }
                        else
                        {
                            Console.WriteLine("\nplease enter a valid input");
                        }
                    }
                } while (validEntry == false);

                switch (choice)
                {
                    //add record
                    case "1":
                        insertHabit();
                        break;

                    //update record
                    case "2":
                        updateHabit();
                        break;

                    //delete record
                    case "3":
                        deleteHabit();
                        break;

                    //display habits
                    case "4":
                        showTrackedHabits();
                        break;

                    //quit app
                    default:
                        choice = "exit";
                        break;

                }
            } while (choice != "exit");
        }

        static void insertHabit()
        {
            bool answerLoop1End = false;
            bool answerLoop2End = false;
            bool answerLoop3End = false;

            string? inputHabit = "";
            string? inputQuantity = "";
            string? inputDate = "00:00:00.000";

            string? habit = "";
            int quantity = 0;
            DateTime date = new DateTime();
            string shortDate = "";

            Console.WriteLine("Enter the name of the Habit you want to track");


            do
            {
                answerLoop1End = false;
                inputHabit = Console.ReadLine();
                if (inputHabit == "")
                {
                    Console.WriteLine("\nplease enter a valid answer");
                }
                else
                {
                    if (inputHabit != null)
                    {
                        habit = inputHabit;
                        answerLoop1End = true;
                    }
                }
            } while (answerLoop1End != true);

            Console.WriteLine("Enter the quantity of the Habit you want to track");

            do
            {
                bool validEntry = false;
                answerLoop2End = false;
                inputQuantity = Console.ReadLine();
                {
                    if (inputQuantity != null)
                    {
                        validEntry = int.TryParse(inputQuantity, out quantity);

                        answerLoop2End = true;
                    }
                    else
                    {
                        Console.WriteLine("\nplease enter a valid quantity");
                    }
                }
            } while (answerLoop2End != true);

            Console.WriteLine("Enter the date in format 'dd/mm/yyyy' of the Habit you want to track");

            do
            {
                bool validEntry = false;
                answerLoop3End = false;
                inputDate = Console.ReadLine();

                {
                    if (inputDate != null)
                    {
                        validEntry = DateTime.TryParse(inputDate, out date);
                        shortDate = date.ToShortDateString();
                        answerLoop3End = true;

                        // testing DateTime dateEntry = DateTime.Now.Date;


                    }
                    else
                    {
                        Console.WriteLine("\nplease enter a valid date");
                    }
                }
            } while (answerLoop3End != true);

            //get required information 
            //run statement to add a row into the table based on prompts 
            using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText =
                @"

                    INSERT INTO TrackedHabits (Habit, Quantity, Date)
                    VALUES (@Habit,@Quantity,@shortDate);
                ";

                //@"INSERT INTO user (name) VALUES ($name)";
                //command.Parameters.AddWithValue("$habit", habit);
                command.Parameters.Add("@Habit", SqliteType.Text).Value = habit;
                command.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
                command.Parameters.Add("@shortDate", SqliteType.Text).Value = shortDate;

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

                /*
                using (SqliteDataReader reader = command.ExecuteReader())
                    {while (reader.Read())
                        {
                            // Process each row
                        }}
                        */
            }

        }

        static void deleteHabit()
        {
            //show habits already tracked
            //pick habit to update by id
            //run statement to delete by id, primary key
            //show results



            //run statement that prints table of habits
            //connection to sqlite database

            Console.WriteLine("Here are the records in HabitTracker.db:");

            using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                //initialize db with id, Habit, Quantity, & Date
                command.CommandText =
                @"
                        SELECT *
                        FROM TrackedHabits
                ";
                //create reader 
                command.ExecuteNonQuery();

                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n{1}: {0}", reader[0], reader.GetName(0));
                            Console.ResetColor();
                            Console.WriteLine("{3}: {0}\n{4}: {1}\n{5}: {2}\n",
                            reader[1], reader[2], reader[3],
                            reader.GetName(1), reader.GetName(2), reader.GetName(3));

                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                command.Dispose();
                connection.Close();
            }

            //Total number of rows, so that loop is maintained
            string? readEntry = "";
            bool validEntry = false;
            int deleteChoice = 0;
            string deleteEntry = "";
            bool exitRowChoiceLoop = false;

            //readEntry = input to console from user
            //deleteEntry = input to parse for int
            //delete choice = int output for row id
            //valid entry is whether or not the delete entry was not null

            do
            {
                Console.WriteLine("Choose the record id of a record below to delete, or write 'return'");

                readEntry = Console.ReadLine();
                if (readEntry != null)
                {
                    deleteEntry = readEntry;
                    if (deleteEntry == "return")
                    {
                        exitRowChoiceLoop = true;
                    }
                    else
                    {
                        validEntry = int.TryParse(deleteEntry, out deleteChoice);

                        try
                        {
                            //using a select statement to find the record, catch the exception and loop back if it doesn't exist
                            using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                            {
                                connection.Open();
                                var command = connection.CreateCommand();

                                //initialize db with id, Habit, Quantity, & Date
                                command.CommandText =
                                @"
                                SELECT *
                                FROM TrackedHabits 
                                WHERE id = @deleteChoice
                                ";

                                command.Parameters.Add("@deleteChoice", SqliteType.Integer).Value = deleteChoice;
                                command.ExecuteNonQuery();

                                try
                                {
                                    using (var reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            int readerTest = reader.GetInt32(0);

                                            if (readerTest == deleteChoice)
                                            {
                                                exitRowChoiceLoop = true;
                                            }

                                            else
                                            {
                                                Console.WriteLine("\nplease enter a valid id or write 'return");
                                                exitRowChoiceLoop = false;
                                            }
                                        }
                                        reader.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        /*
                        if (deleteChoice <= rowTotalLoop)
                        {exitRowChoiceLoop = true;}
                        else
                        {Console.WriteLine("\nplease enter a valid id or write 'return");
                            exitRowChoiceLoop = false;}
                        */
                    }
                }
                else
                {
                    Console.WriteLine("\nplease enter a valid id or write 'return");
                    exitRowChoiceLoop = false;

                }

            } while (exitRowChoiceLoop != true);


            if (deleteEntry != "return")

            {
                //use deleteEntry as string to get into the while loop, then use deleteChoice

                using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                    DELETE from TrackedHabits where id = @deleteChoice
                    ";
                    //@"INSERT INTO user (name) VALUES ($name)";
                    //command.Parameters.AddWithValue("$habit", habit);

                    command.Parameters.Add("@deleteChoice", SqliteType.Integer).Value = deleteChoice;

                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                }
            }
        }

        static void updateHabit()
        {
            //show habits
            //pick habit to update by id
            //choose which value to update
            //run statement to update specific value at row id in table

            {
                //show habits already tracked
                //pick habit to update by id
                //run statement to delete by id, primary key
                //show results



                //run statement that prints table of habits
                //connection to sqlite database

                Console.WriteLine("Here are the records in HabitTracker.db:");

                using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    //initialize db with id, Habit, Quantity, & Date
                    command.CommandText =
                    @"
                        SELECT *
                        FROM TrackedHabits
                ";
                    //create reader 
                    command.ExecuteNonQuery();

                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n{1}: {0}", reader[0], reader.GetName(0));
                                Console.ResetColor();
                                Console.WriteLine("{3}: {0}\n{4}: {1}\n{5}: {2}\n",
                                reader[1], reader[2], reader[3],
                                reader.GetName(1), reader.GetName(2), reader.GetName(3));

                            }

                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    command.Dispose();
                    connection.Close();
                }

                //Total number of rows, so that loop is maintained
                string? readEntry = "";
                bool validEntry = false;
                int updateChoice = 0;
                string updateEntry = "";
                bool exitRowChoiceLoop = false;



                //readEntry = input to console from user
                //deleteEntry = input to parse for int
                //delete choice = int output for row id
                //valid entry is whether or not the delete entry was not null

                do
                {
                    Console.WriteLine("Choose the record id of a record below to update, or write 'return'");

                    readEntry = Console.ReadLine();
                    if (readEntry != null)
                    {
                        updateEntry = readEntry;
                        if (updateEntry == "return")
                        {
                            exitRowChoiceLoop = true;
                        }
                        else
                        {
                            validEntry = int.TryParse(updateEntry, out updateChoice);

                            try
                            {
                                //using a select statement to find the record, catch the exception and loop back if it doesn't exist
                                using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                                {
                                    connection.Open();
                                    var command = connection.CreateCommand();

                                    //initialize db with id, Habit, Quantity, & Date
                                    command.CommandText =
                                    @"
                                SELECT *
                                FROM TrackedHabits 
                                WHERE id = @updateChoice
                                ";

                                    command.Parameters.Add("@updateChoice", SqliteType.Integer).Value = updateChoice;
                                    command.ExecuteNonQuery();

                                    try
                                    {
                                        using (var reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                int readerTest = reader.GetInt32(0);

                                                if (readerTest == updateChoice)
                                                {
                                                    exitRowChoiceLoop = true;
                                                }

                                                else
                                                {
                                                    Console.WriteLine("\nplease enter a valid id or write 'return");
                                                    exitRowChoiceLoop = false;
                                                }
                                            }
                                            reader.Close();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("\nplease enter a valid id or write 'return");
                        exitRowChoiceLoop = false;

                    }

                } while (exitRowChoiceLoop != true);


                string updateColumn = "";


                if (updateEntry != "return")
                {
                    //use deleteEntry as string to get into the while loop, then use deleteChoice
                    //ask what column to update, then set it to a variable
                    string updateMenu = "";

                    do
                    {

                        Console.WriteLine("Select the column by number below that needs to be updated (1-3), or type 'exit'");
                        Console.WriteLine("1. Habit \n2. Quantity \n3. Date");
                        bool validUpdateEntry = false;

                        string? inputChoice = "";
                        do
                        {
                            inputChoice = Console.ReadLine();
                            {
                                if (inputChoice != null && (inputChoice == "1" || inputChoice == "2" || inputChoice == "3" || inputChoice == "4" || inputChoice == "exit"))
                                {
                                    //if (inputChoice == "1" || inputChoice == "2" || inputChoice == "3" || inputChoice == "4")
                                    updateMenu = inputChoice;
                                    validUpdateEntry = true;
                                }
                                else
                                {
                                    Console.WriteLine("\nplease choose a column");
                                }
                            }
                        } while (validUpdateEntry == false);

                        switch (updateMenu)
                        {
                            //set column variable to 1 which is Habit
                            case "1":
                                updateColumn = "Habit";
                                updateMenu = "exit";
                                break;

                            //set column variable to 2 which is Quantity
                            case "2":
                                updateColumn = "Quantity";
                                updateMenu = "exit";
                                break;

                            //set column variable to 1 which is Date
                            case "3":
                                updateColumn = "Date";
                                updateMenu = "exit";
                                break;

                            //quit app
                            default:
                                updateMenu = "exit";
                                break;
                        }
                    } while (updateMenu != "exit");



                    bool answerLoop1End = false;
                    bool answerLoop2End = false;
                    bool answerLoop3End = false;

                    string? inputHabit = "";
                    string? inputQuantity = "";
                    string? inputDate = "00:00:00.000";

                    string? updateHabit = "";
                    int updateQuantity = 0;
                    DateTime updateDate = new DateTime();
                    string shortUpdateDate = "";


                    if (updateColumn == "Habit")
                    {
                        Console.WriteLine($"Enter the new name to update '{updateColumn}'");
                        do
                        {
                            answerLoop1End = false;
                            inputHabit = Console.ReadLine();
                            if (inputHabit == "")
                            {
                                Console.WriteLine("\nplease enter a valid answer");
                            }
                            else
                            {
                                if (inputHabit != null)
                                {
                                    updateHabit = inputHabit;
                                    answerLoop1End = true;
                                }
                            }
                        } while (answerLoop1End != true);

                        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText =
                            @"
                            UPDATE TrackedHabits SET Habit = @updateHabit WHERE id = @updateChoice
                            ";
                            //@"INSERT INTO user (name) VALUES ($name)";
                            //command.Parameters.AddWithValue("$habit", habit);

                            command.Parameters.Add("@updateHabit", SqliteType.Text).Value = updateHabit;
                            command.Parameters.Add("@updateChoice", SqliteType.Integer).Value = updateChoice;

                            command.ExecuteNonQuery();
                            command.Dispose();
                            connection.Close();
                        }
                    }

                    if (updateColumn == "Quantity")
                    {
                        Console.WriteLine($"Enter the new quantity to update '{updateColumn}'");
                        do
                        {
                            bool validUpdateEntry = false;
                            answerLoop2End = false;
                            inputQuantity = Console.ReadLine();
                            {
                                if (inputQuantity != null)
                                {
                                    validUpdateEntry = int.TryParse(inputQuantity, out updateQuantity);

                                    answerLoop2End = true;
                                }
                                else
                                {
                                    Console.WriteLine("\nplease enter a valid quantity");
                                }
                            }
                        } while (answerLoop2End != true);

                        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText =
                            @"
                            UPDATE TrackedHabits SET Quantity = @updateQuantity WHERE id = @updateChoice
                            ";
                            //@"INSERT INTO user (name) VALUES ($name)";
                            //command.Parameters.AddWithValue("$habit", habit);

                            command.Parameters.Add("@updateQuantity", SqliteType.Integer).Value = updateQuantity;
                            command.Parameters.Add("@updateChoice", SqliteType.Integer).Value = updateChoice;

                            command.ExecuteNonQuery();
                            command.Dispose();
                            connection.Close();
                        }

                    }

                    if (updateColumn == "Date")
                    {
                        Console.WriteLine($"Enter the new date in format 'yyyy-mm-dd hh:mm:ss' to update '{updateColumn}'");
                        do
                        {
                            bool validUpdateEntry = false;
                            answerLoop3End = false;
                            inputDate = Console.ReadLine();
                            {
                                if (inputDate != null)
                                {
                                    validUpdateEntry = DateTime.TryParse(inputDate, out updateDate);
                                    shortUpdateDate = updateDate.ToShortDateString();
                                    answerLoop3End = true;
                                }
                                else
                                {
                                    Console.WriteLine("\nPlease enter a valid date");
                                }
                            }
                        } while (answerLoop3End != true);

                        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
                        {
                            connection.Open();
                            var command = connection.CreateCommand();
                            command.CommandText =
                            @"
                            UPDATE TrackedHabits SET Date = @shortUpdateDate WHERE id = @updateChoice
                            ";
                            //@"INSERT INTO user (name) VALUES ($name)";
                            //command.Parameters.AddWithValue("$habit", habit);

                            command.Parameters.Add("@shortUpdateDate", SqliteType.Text).Value = shortUpdateDate;
                            command.Parameters.Add("@updateChoice", SqliteType.Integer).Value = updateChoice;

                            command.ExecuteNonQuery();
                            command.Dispose();
                            connection.Close();
                        }
                    }
                }
            }
        }

        static void showTrackedHabits()
        {
            //run statement that prints table of habits

            //connection to sqlite database
            using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                //initialize db with id, Habit, Quantity, & Date
                command.CommandText =
                @"
                        SELECT *
                        FROM TrackedHabits
                ";
                //create reader 
                command.ExecuteNonQuery();

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n{1}: {0}", reader[0], reader.GetName(0));
                            Console.ResetColor();
                            Console.WriteLine("{3}: {0}\n{4}: {1}\n{5}: {2}\n",
                            reader[1], reader[2], reader[3],
                            reader.GetName(1), reader.GetName(2), reader.GetName(3));

                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Press any key to return to the main menu");
                Console.ReadLine();

                command.Dispose();
                connection.Close();
            }
        }
    }
}



/*
using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
{
    connection.Open();
    var command = connection.CreateCommand();
    command.CommandText =
    @"
        UPDATE TrackedHabits SET @updateColumn = @updateValue WHERE id = @updateChoice
    ";
    //@"INSERT INTO user (name) VALUES ($name)";
    //command.Parameters.AddWithValue("$habit", habit);
    command.Parameters.Add("@updateColumn", SqliteType.Integer).Value = updateColumn;
    command.Parameters.Add("@updateValue", SqliteType.Integer).Value = updateValue;
    command.Parameters.Add("@updateChoice", SqliteType.Integer).Value = updateChoice;

    command.ExecuteNonQuery();
    command.Dispose();
    connection.Close();
}
*/

/*
while (reader.Read())
{



    Console.WriteLine("{4}: {0}\n{5}: {1}\n{6}: {2}\n{7}: {3}\n",
    reader[0], reader[1], reader[2], reader[3],
    reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
    rowTotalLoop++;
}
*/
/*
namespace HelloWorldSample
{
    class Program
    {
        static void Main()
        {
            using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();


                command.CommandText =
                @"
                    CREATE TABLE user (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        Habit TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,

                    );

                    INSERT INTO user
                    VALUES (1, 'Brice'),
                           (2, 'Alexander'),
                           (3, 'Nate');
                ";
                command.ExecuteNonQuery();

                Console.Write("Name: ");
                var name = Console.ReadLine();

                #region snippet_Parameter
                command.CommandText =
                @"
                    INSERT INTO user (name)
                    VALUES ($name)
                ";
                command.Parameters.AddWithValue("$name", name);
                #endregion
                command.ExecuteNonQuery();
                command.Dispose();

                command.CommandText =
                @"
                    SELECT last_insert_rowid()
                ";
                var newId = (long)command.ExecuteScalar();

                Console.WriteLine($"Your new user ID is {newId}.");
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();




            }

            Console.Write("User ID: ");
            var id = int.Parse(Console.ReadLine());

            #region snippet_HelloWorld
            using (var connection = new SqliteConnection("Data Source=hello.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT name
                    FROM user
                    WHERE id = $id
                ";
                command.Parameters.AddWithValue("$id", id);
                command.ExecuteNonQuery();
                command.Dispose();


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);

                        Console.WriteLine($"Hello, {name}!");
                    }
                    reader.Close();
                }
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();

            }
            #endregion
            //GC.Collect();
            // GC.WaitForPendingFinalizers();
            // Clean up
            File.Delete("hello.db");
        }
    }


}
*/
/*
public static class ClearSQLiteCommandConnectionHelper
{
    private static readonly List<SQLiteCommand> OpenCommands = new List<SQLiteCommand>();

    public static void Initialise()
    {
        SQLiteConnection.Changed += SqLiteConnectionOnChanged;
    }

    private static void SqLiteConnectionOnChanged(object sender, ConnectionEventArgs connectionEventArgs)
    {
        if (connectionEventArgs.EventType == SQLiteConnectionEventType.NewCommand && connectionEventArgs.Command is SQLiteCommand)
        {
            OpenCommands.Add((SQLiteCommand)connectionEventArgs.Command);
        }
        else if (connectionEventArgs.EventType == SQLiteConnectionEventType.DisposingCommand && connectionEventArgs.Command is SQLiteCommand)
        {
            OpenCommands.Remove((SQLiteCommand)connectionEventArgs.Command);
        }

        if (connectionEventArgs.EventType == SQLiteConnectionEventType.Closed)
        {
            var commands = OpenCommands.ToList();
            foreach (var cmd in commands)
            {
                if (cmd.Connection == null)
                {
                    OpenCommands.Remove(cmd);
                }
                else if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection = null;
                    OpenCommands.Remove(cmd);
                }
            }
        }
    }
}

*/

