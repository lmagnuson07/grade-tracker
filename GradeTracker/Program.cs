using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SemesterGradeTracker
{
    // Edited write to courses file
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder classGradesFile = new StringBuilder("C:\\Users\\logma\\OneDrive - NAIT\\Documents\\Semester Tracker Files\\Marks-List\\Grades-Course-1.txt");
            const string CLASS_FILE_NAME = "C:\\Users\\logma\\OneDrive - NAIT\\Documents\\Semester Tracker Files\\Course-Names.txt";
            const string MAX_CLASS_FILE = "C:\\Users\\logma\\OneDrive - NAIT\\Documents\\Semester Tracker Files\\Max-Class.txt";
            List<Courses> aCourses = new List<Courses>();
            List<Grades> aGrades = new List<Grades>(aCourses.Count);
            Grades defaultGrade = new Grades("/~~/~/~///~/~/", 0, 0);
            bool programLoop = true;
            char ynChoice = ' ', menuChoice = ' ';
            int maxClasses = 0, classChoiceIndex = -1;
            SetMaxClasses(aCourses, MAX_CLASS_FILE, '0', ref maxClasses, ref programLoop);
            if (programLoop)
            {
                try
                {
                    // If the file doesnt exists, throws an exception
                    ReadFromCoursesFile(aCourses, CLASS_FILE_NAME);
                    if (aCourses.Count > maxClasses)
                    {
                        Console.WriteLine("\tThere are more records on the file than the maximum number of classes");
                        Console.WriteLine("\tY - Clear the file and start with a clean slate");
                        Console.WriteLine("\tN - Exit the program and edit the files yourself");
                        ynChoice = GetYN("\tMake a choice[y/n] ");
                        if (ynChoice == 'y')
                        {
                            throw new Exception();
                        }
                        else
                        {
                            programLoop = false;
                        }
                    }
                }
                catch
                {
                    if (aCourses.Count > maxClasses)
                    {
                        aCourses.Clear();
                    }
                    // creates an empty courses file. 
                    WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                }
            }
            string title = "\n\t************* Semester Grade Tracker *************";
            string subTitle = "\n\t******** Lets initialize those classes!! *********";
            string selectClassPrompt = "\n\t************ Select a class *****************";
            bool bLoop = true, bIndexSelected = false;
            while (programLoop)
            {
                if (aCourses.Count < 1)
                {
                    InitializeClasses(aCourses, defaultGrade, classGradesFile, CLASS_FILE_NAME, maxClasses, (title + subTitle));
                }
                while (bLoop)
                {
                    Console.WriteLine("\n\t************* Semester Grade Tracker *************");
                    Console.WriteLine(classChoiceIndex > -1 && bIndexSelected ? $"\tSelected class: {aCourses[classChoiceIndex].GetClassID()} - {aCourses[classChoiceIndex].GetClassName()} | " +
                        $"Weight: {aCourses[classChoiceIndex].GetTotalWeightedMark():N2}/{aCourses[classChoiceIndex].GetTotalWeight():N2}\n" : "\tSelect a class to continue. (Option 1) ...\n");
                    menuChoice = GetCourseMenuChoice();
                    switch (menuChoice)
                    {
                        case '1': // Choose a class
                            {
                                Console.Clear();
                                classChoiceIndex = SelectClass(aCourses, aGrades, classGradesFile, ref bIndexSelected, selectClassPrompt);
                                break;
                            }
                        case '2': // Initialize classes
                            {
                                InitializeClasses(aCourses, defaultGrade, classGradesFile, CLASS_FILE_NAME, maxClasses, subTitle);
                                break;
                            }
                        case '3': // edit/overwrite class name/ID
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class yet");
                                }
                                else
                                {
                                    Console.Clear();
                                    EditSelectedClassNameID(aCourses, classChoiceIndex);
                                }
                                break;
                            }
                        case '4': // wipe the class from file
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class yet");
                                }
                                else
                                {
                                    Console.Clear();
                                    WipeSelectedClass(aCourses, aGrades, classGradesFile, CLASS_FILE_NAME, classChoiceIndex, ref bIndexSelected);
                                }
                                break;
                            }
                        case '5': // proceed to next menu
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class yet");
                                }
                                else
                                {
                                    Console.Clear();
                                    bLoop = false;
                                }
                                break;
                            }
                        case '6': // change order of classes
                            {
                                Console.Clear();
                                if (aCourses.Count < 2)
                                {
                                    Console.WriteLine("\tYou must have at least 2 classes to perform a swap");
                                }
                                else
                                {
                                    Console.WriteLine("\tEnter 0 to cancel the operation");
                                    DisplayClassMenu(aCourses);
                                    SwapClasses(aCourses, aGrades, classGradesFile, CLASS_FILE_NAME, classChoiceIndex);
                                }
                                break;
                            }
                        case '7': // change max classes
                            {
                                SetMaxClasses(aCourses, MAX_CLASS_FILE, menuChoice, ref maxClasses, ref programLoop);
                                break;
                            }
                        case '~': // clear all classes for semester
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class");
                                }
                                else
                                {
                                    Console.WriteLine();
                                    WipeAllClasses(aCourses, aGrades, classGradesFile, CLASS_FILE_NAME, ref bIndexSelected);
                                }
                                break;
                            }
                        case 'x':
                        case 'X':// exit program
                            {
                                Console.Clear();
                                bLoop = false;
                                programLoop = false;
                                break;
                            }
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\tInvalid menu choice, try again...");
                                break;
                            }
                    }
                }
                menuChoice = ' ';
                while (menuChoice != 'x' && programLoop)
                {
                    Console.WriteLine("\n\t************* Semester Grade Tracker *************");
                    Console.WriteLine($"\tSelected class: {aCourses[classChoiceIndex].GetClassID()} - {aCourses[classChoiceIndex].GetClassName()} | " +
                        $"Weight: {aCourses[classChoiceIndex].GetTotalWeightedMark():N2}/{aCourses[classChoiceIndex].GetTotalWeight():N2}");
                    menuChoice = GetGradesMenuChoice();
                    switch (menuChoice)
                    {
                        case '1': // select class to edit
                            {
                                Console.Clear();
                                classChoiceIndex = SelectClass(aCourses, aGrades, classGradesFile, ref bIndexSelected, selectClassPrompt);
                                break;
                            }
                        case '2': // add a to selected class
                            {
                                AddMarksToClass(aCourses, aGrades, classGradesFile, CLASS_FILE_NAME, classChoiceIndex);
                                break;
                            }
                        case '3': // Display everything all the selected grades, and my marks.
                            {
                                if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tThere are no marks on the current class yet...");
                                }
                                else
                                {
                                    Console.Clear();
                                    DisplayGradesMenu(aCourses, aGrades, classChoiceIndex);
                                    Console.WriteLine("\n\tPress enter to continue...");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                break;
                            }
                        case '4': // edit selected classes marks 
                            {
                                if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tThere are no marks on the current class yet...");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine();
                                    DisplayGrades(aGrades, "\tChoose a mark to edit...\n");
                                    EditSelectedClassesMarks(aCourses, aGrades, classGradesFile, CLASS_FILE_NAME, classChoiceIndex);
                                }
                                break;
                            }
                        case '5': // Go back to last menu
                            {
                                menuChoice = 'x';
                                programLoop = true;
                                bLoop = true;
                                Console.Clear();
                                break;
                            }
                        case '6': // change order of grades
                            {
                                Console.Clear();
                                Console.WriteLine("\tEnter 0 to cancel the operation");
                                DisplayGradesMenu(aCourses, aGrades, classChoiceIndex);
                                SwapSelectedClassesMarks(aGrades, classGradesFile, classChoiceIndex);
                                break;
                            }
                        case '7': // Remove selected classes marks 
                            {
                                if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tThere are no marks on the current class yet...");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine();
                                    DisplayGrades(aGrades, "\tChoose a mark to remove...");
                                    WipeSelectedClassesMarks(aCourses, aGrades, defaultGrade, classGradesFile, CLASS_FILE_NAME, classChoiceIndex);
                                }
                                break;
                            }
                        case '~': // Wipe selected classes marks
                            {
                                if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tThere are no marks on the current class yet...");
                                }
                                else
                                {
                                    Console.Clear();
                                    DisplayGrades(aGrades, "");
                                    WipeAllSelectedClassesMarks(aCourses, aGrades, defaultGrade, classGradesFile, CLASS_FILE_NAME, classChoiceIndex);
                                }
                                break;
                            }
                        case 'x':
                        case 'X':// exit program
                            {
                                programLoop = false;
                                Console.Clear();
                                break;
                            }
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("\tInvalid menu choice, try again...");
                                break;
                            }
                    }
                }
            }
            Console.WriteLine("\n\t*************** Thanks for using ***************");
            Console.WriteLine("\t*************** Semester Tracker ***************");
            Console.WriteLine("\t****         Have a wonderful day!!         ****");
            Console.WriteLine("\n\n\t              Press enter to exit");
            Console.ReadLine();
        }
        // Class Menu Methods
        static int SelectClass(List<Courses> aCourses, List<Grades> aGrades, StringBuilder classGradesFile, ref bool bIndexSelected, string prompt)
        {
            bool bLoop = true;
            int classChoiceIndex = 0;
            while (bLoop)
            {
                Console.WriteLine(prompt);
                DisplayClassMenu(aCourses);
                int menuChoice = GetValidInt($"\n\tChoose a class >> ");
                if (menuChoice > 0 && menuChoice <= aCourses.Count)
                {
                    classChoiceIndex = menuChoice - 1;
                    ReadFromGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), menuChoice.ToString(),
                                    classGradesFile.Length - 5, menuChoice < 10 ? 1 : 2).ToString());
                    bIndexSelected = true;
                    bLoop = false;
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nInvalid menu choice, try again...");
                }
            }
            return classChoiceIndex;
        }
        static void InitializeClasses(List<Courses> aCourses, Grades defaultGrade, StringBuilder classGradesFile, string classFile, int maxClasses, string prompt)
        {
            List<Grades> aGradesTemp = new List<Grades>();
            string className = "", classID = "";
            char ynChoice = ' ';
            bool bLoop = true;
            while (aCourses.Count < maxClasses && bLoop)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine(prompt);
                    Console.WriteLine("\n\tYou are at {1} of {2} classes", "", aCourses.Count, maxClasses);
                    className = GetValidString("\tEnter the class name >> ");
                    classID = GetValidString("\tEnter the class ID >> ");
                    Courses course = new Courses(className, classID);
                    aCourses.Add(course);
                    aGradesTemp.Add(defaultGrade);
                    WriteToGradesFile(aGradesTemp, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), aCourses.Count.ToString(),
                        classGradesFile.Length - 5, aCourses.Count < 10 ? 1 : 2).ToString());
                    aGradesTemp.Clear();
                    ynChoice = GetYN("\tDo you want to enter another class? [y/n] ");
                    if (ynChoice == 'n')
                    {
                        bLoop = false;
                    }
                    WriteToCoursesFile(aCourses, classFile);
                    Console.Clear();
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("\t{0}", e.Message);
                }
            }
            if (aCourses.Count >= maxClasses)
            {
                Console.Clear();
                Console.WriteLine("\tYou've entered the maximum number of classes...\n\tWipe the semester or delete a class.");
            }
        }
        static void EditSelectedClassNameID(List<Courses> aCourses, int classChoiceIndex)
        {
            string className = "", classID = "";
            Console.WriteLine("\tYou are about to edit {0} - {1} | Weight: {2}/{3}", aCourses[classChoiceIndex].GetClassID(), aCourses[classChoiceIndex].GetClassName(),
                aCourses[classChoiceIndex].GetTotalWeightedMark(), aCourses[classChoiceIndex].GetTotalWeight());
            // ask to continue
            char ynChoice = GetYN("\tContinue? [y/n] ");
            if (ynChoice == 'y')
            {
                bool bLoop = true;
                while (bLoop)
                {
                    className = GetValidString("\tEnter the new class name >> ");
                    classID = GetValidString("\tEnter the new class ID >> ");
                    try
                    {
                        Courses course = new Courses(className, classID);
                        bLoop = false;
                        // add object back to list and write to file
                        aCourses[classChoiceIndex] = course;
                        WriteToCoursesFile(aCourses, className);
                        Console.Clear();
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Console.WriteLine("\t{0}", e.Message);
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\tYou did not edit the selected class...");
            }
        }
        static void SwapClasses(List<Courses> aCourses, List<Grades> aGrades, StringBuilder classGradesFile, string classFile, int classChoiceIndex)
        {
            int classOne = GetValidInt("\n\tSelect a class to move >> ") - 1;
            int classTwo = GetValidInt("\n\tSelect where you want to move it\n\t(The two classes will be swapped!) >> ") - 1;
            if (classOne > -1 && classOne <= (aCourses.Count - 1) && classTwo > -1 && classTwo <= (aCourses.Count - 1) && classOne != classTwo)
            {
                Courses temp = aCourses[classOne];
                aCourses[classOne] = aCourses[classTwo];
                aCourses[classTwo] = temp;
                WriteToCoursesFile(aCourses, classFile);
                // swap the grades files
                List<Grades> aTempGrades = new List<Grades>();
                ReadFromGradesFile(aTempGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classOne + 1).ToString(),
                    classGradesFile.Length - 5, classOne + 1 < 10 ? 1 : 2).ToString());
                ReadFromGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classTwo + 1).ToString(),
                    classGradesFile.Length - 5, classTwo + 1 < 10 ? 1 : 2).ToString());
                WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classOne + 1).ToString(),
                    classGradesFile.Length - 5, classOne + 1 < 10 ? 1 : 2).ToString());
                WriteToGradesFile(aTempGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classTwo + 1).ToString(),
                    classGradesFile.Length - 5, classTwo + 1 < 10 ? 1 : 2).ToString());
                if (classChoiceIndex != -1)
                {
                    ReadFromGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                    classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString());
                }
                Console.Clear();
                Console.WriteLine("\tThe classes were successfully swapped");
            }
            else
            {
                if (classOne >= aCourses.Count || classTwo >= aCourses.Count)
                {
                    Console.Clear();
                    Console.WriteLine("\tOne of your choices was greater than the amount of courses...");
                }
                else if (classOne == classTwo)
                {
                    Console.Clear();
                    Console.WriteLine("\tYour choices were the same, therefore nothing was swapped...");
                }
                else if (classOne < -1 || classTwo < -1)
                {
                    Console.Clear();
                    Console.WriteLine("\tYou entered a negative number, so nothing happened...");
                }
                else
                {
                    Console.Clear();
                }
            }
        }
        static void WipeSelectedClass(List<Courses> aCourses, List<Grades> aGrades, StringBuilder classGradesFile, string classFile, int classChoiceIndex, ref bool bIndexSelected)
        {
            List<Grades> aGradesTemp = new List<Grades>();
            bool bLoop = true;
            Console.WriteLine("\tYou are about to remove {0} - {1} | Weight: {2}/{3}", aCourses[classChoiceIndex].GetClassID(), aCourses[classChoiceIndex].GetClassName(),
                aCourses[classChoiceIndex].GetTotalWeightedMark(), aCourses[classChoiceIndex].GetTotalWeight());
            while (bLoop)
            {
                try
                {
                    char ynChoice = GetYN("\tAre you sure you want to remove this class? [y/n] ");
                    if (ynChoice == 'n')
                    {
                        bLoop = false;
                    }
                    else
                    {
                        aCourses.RemoveAt(classChoiceIndex);
                        bIndexSelected = false;
                        bLoop = false;
                        // write new array to class
                        WriteToCoursesFile(aCourses, classFile);
                        // overwrite the deleted class grade file with nothing.
                        aGrades.Clear();
                        // clears the selected classes grades
                        WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                            classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString());
                        // Shifts data on the files over 
                        for (int i = classChoiceIndex; i < aCourses.Count - 1; i++)
                        {
                            ReadFromGradesFile(aGradesTemp, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (i + 2).ToString(),
                                classGradesFile.Length - 5, i + 2 < 10 ? 1 : 2).ToString());
                            WriteToGradesFile(aGradesTemp, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (i + 1).ToString(),
                                classGradesFile.Length - 5, i + 1 < 10 ? 1 : 2).ToString());
                        }
                        // after everything has been shifted, make the last file empty
                        aGradesTemp.Clear();
                        WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (aCourses.Count + 1).ToString(),
                            classGradesFile.Length - 5, aCourses.Count + 1 < 10 ? 1 : 2).ToString());
                        Console.Clear();
                    }
                }
                catch (Exception e)
                {
                    aGradesTemp.Clear();
                    Console.WriteLine("\t{0}", e.Message);
                }
            }
        }
        static void WipeAllClasses(List<Courses> aCourses, List<Grades> aGrades, StringBuilder classGradesFile, string classFile, ref bool bIndexSelected)
        {
            bool bLoop = true;
            while (bLoop)
            {
                Console.Clear();
                char ynChoice = GetYN("\tAre you sure you want to remove all the classes from this semester?\n\tOnce you do this, you can't revert back [y/n] ");
                if (ynChoice == 'n')
                {
                    bLoop = false;
                }
                else
                {
                    bLoop = false;
                    bIndexSelected = false;
                    // write new empty array to file
                    aGrades.Clear();
                    int temp = 0;
                    foreach (var item in aCourses)
                    {
                        // wipes each grades file.
                        temp++;
                        WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), temp.ToString(),
                            classGradesFile.Length - 5, temp < 10 ? 1 : 2).ToString().ToString());
                    }
                    aCourses.Clear();
                    WriteToCoursesFile(aCourses, classFile);
                    Console.Clear();
                }
            }
        }
        // Grades Menu Methods
        static void AddMarksToClass(List<Courses> aCourses, List<Grades> aGrades, StringBuilder classGradesFile, string classFile, int classChoiceIndex)
        {
            string assessmentName = "";
            double maxMark = 0, userGrade = 0, weight = 0;
            bool bLoop = true;
            while (bLoop && aCourses[classChoiceIndex].GetTotalWeight() < 100)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("\tEnter an assessment name, the maximum mark, the weight, and your grade...");
                    assessmentName = GetValidString("\tEnter the assessment name >> ");
                    maxMark = GetValidDouble("\tEnter the maximum mark >> ");
                    weight = GetValidDouble("\tEnter the weight >> ");
                    userGrade = GetValidDouble("\tEnter your mark >> ");
                    Console.WriteLine("\t{0}: {1:N2}/{2:N2} - Weight: {3:N2}", assessmentName, userGrade, maxMark, weight);
                    char ynChoice = GetYN("\tContinue to save? [y/n] ");
                    if (ynChoice == 'y')
                    {
                        // clears array if the default values are given
                        if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                        {
                            aGrades.Clear();
                        }
                        Grades grade = new Grades(assessmentName, maxMark, weight);
                        grade.SetUserMark(userGrade);
                        aCourses[classChoiceIndex].SetTotalWeight(weight);
                        try
                        {
                            aCourses[classChoiceIndex].SetTotalWeightedMark(grade.GetWeightedMark());
                        }
                        catch
                        {
                            aCourses[classChoiceIndex].SetTotalWeight(weight * -1);
                            throw new OverflowException();
                        }
                        aGrades.Add(grade);
                        WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                        WriteToCoursesFile(aCourses, classFile);
                        bLoop = false;
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        bLoop = false;
                    }
                }
                catch (Exception e)
                {
                    if (e is FormatException || e is OverflowException)
                    {
                        bLoop = false;
                        Console.Clear();
                        Console.WriteLine("\t{0}", e.Message);
                    }
                    else
                    {
                        bLoop = false;
                        aCourses[classChoiceIndex].SetTotalWeight(weight * -1);
                        aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[aGrades.Count - 1].GetWeightedMark() * -1);
                        aGrades.RemoveAt(aGrades.Count - 1);
                        WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                        WriteToCoursesFile(aCourses, classFile);
                        Console.Clear();
                        Console.WriteLine("\t{0}", e.Message);
                    }
                }
            }
            if (aCourses[classChoiceIndex].GetTotalWeight() > 100)
            {
                Console.Clear();
                Console.WriteLine("\tThe total weight is already 100, and cannot be higher...");
            }
        }
        static void EditSelectedClassesMarks(List<Courses> aCourses, List<Grades> aGrades, StringBuilder classGradesFile, string classFile, int classChoiceIndex)
        {
            bool bLoop = true;
            string assessmentName = "";
            double maxMark = 0, weight = 0, userGrade = 0;
            while (bLoop)
            {
                int intMenuChoice = GetValidInt("\n\tEnter a menu choice >> ");
                if (intMenuChoice > 0 && intMenuChoice <= aGrades.Count + 1)
                {
                    Console.Clear();
                    Console.WriteLine("\t{0}: {1:N2}/{2:N2} = {3:N2}%, weighted: {4:N2}% ({5:N2}% weight)", aGrades[intMenuChoice - 1].GetAssesmentName(),
                                                                                aGrades[intMenuChoice - 1].GetUserMark(),
                                                                                aGrades[intMenuChoice - 1].GetMaxMark(),
                                                                                aGrades[intMenuChoice - 1].GetMarkPercentage(),
                                                                                aGrades[intMenuChoice - 1].GetWeightedMark(),
                                                                                aGrades[intMenuChoice - 1].GetWeight());
                    try
                    {
                        // Format exception
                        assessmentName = GetValidString("\tEnter the new assessment name >> ");
                        maxMark = GetValidDouble("\tEnter the new maximum mark >> ");
                        weight = GetValidDouble("\tEnter the new weight >> ");
                        userGrade = GetValidDouble("\tEnter the new mark >> ");
                        Console.Clear();
                        Console.WriteLine("\t{0}: {1:N2}/{2:N2} - Weight: {3:N2}", assessmentName, userGrade, maxMark, weight);
                        char ynChoice = GetYN("\tContinue to save? [y/n] ");
                        if (ynChoice == 'y')
                        {
                            // Format exception
                            Grades grade = new Grades(assessmentName, maxMark, weight);
                            grade.SetUserMark(userGrade);
                            try
                            {
                                aCourses[classChoiceIndex].SetTotalWeight(aGrades[intMenuChoice - 1].GetWeight() * -1); // would throw exception if below 0
                            }
                            catch
                            {
                                throw new Exception();
                            }
                            try
                            {
                                aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[intMenuChoice - 1].GetWeightedMark() * -1);
                            }
                            catch
                            {
                                aCourses[classChoiceIndex].SetTotalWeight(aGrades[intMenuChoice - 1].GetWeight());
                                throw new Exception();
                            }
                            try
                            {
                                aCourses[classChoiceIndex].SetTotalWeight(weight);
                            }
                            catch
                            {
                                aCourses[classChoiceIndex].SetTotalWeight(aGrades[intMenuChoice - 1].GetWeight());
                                aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[intMenuChoice - 1].GetWeightedMark());
                                throw new Exception();
                            }
                            try
                            {
                                aCourses[classChoiceIndex].SetTotalWeightedMark(grade.GetWeightedMark());
                            }
                            catch
                            {
                                aCourses[classChoiceIndex].SetTotalWeight(aGrades[intMenuChoice - 1].GetWeight());
                                aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[intMenuChoice - 1].GetWeightedMark());
                                aCourses[classChoiceIndex].SetTotalWeight(weight * -1);
                                throw new Exception();
                            }
                            aGrades[intMenuChoice - 1] = grade;
                            WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                            WriteToCoursesFile(aCourses, classFile);
                            bLoop = false;
                            Console.Clear();
                        }
                        else
                        {
                            Console.Clear();
                            bLoop = false;
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is FormatException || e is OverflowException)
                        {
                            bLoop = false;
                            Console.Clear();
                            Console.WriteLine("\t{0}", e.Message);
                        }
                        else
                        {
                            bLoop = false;
                            Console.Clear();
                            aCourses[classChoiceIndex].SetTotalWeight(aGrades[intMenuChoice - 1].GetWeight() * -1);
                            aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[intMenuChoice - 1].GetWeightedMark() * -1);
                            aGrades.RemoveAt(intMenuChoice - 1);
                            WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                            WriteToCoursesFile(aCourses, classFile);
                            Console.WriteLine("\tThere was an error adding the grades to the file, and you may have lost a record in the process...");
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\tInvalid menu choice... try again...");
                }
            }
        }
        static void SwapSelectedClassesMarks(List<Grades> aGrades, StringBuilder classGradesFile, int classChoiceIndex)
        {
            int gradeOne = GetValidInt("\n\tSelect a grade to move >> ") - 1;
            int gradeTwo = GetValidInt("\n\tSelect where you want to move it\n\t(The two grades will be swapped!) >> ") - 1;
            if (gradeOne > -1 && gradeOne <= (aGrades.Count - 1) && gradeTwo > -1 && gradeTwo <= (aGrades.Count - 1) && gradeOne != gradeTwo)
            {
                Grades temp = aGrades[gradeOne];
                aGrades[gradeOne] = aGrades[gradeTwo];
                aGrades[gradeTwo] = temp;
                WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                        classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString());
                Console.Clear();
                Console.WriteLine("\tThe grades were successfully swapped");
            }
            else
            {
                if (gradeOne >= aGrades.Count || gradeTwo >= aGrades.Count)
                {
                    Console.Clear();
                    Console.WriteLine("\tOne of your choices was greater than the amount of grades...");
                }
                else if (gradeOne == gradeTwo)
                {
                    Console.Clear();
                    Console.WriteLine("\tYour choices were the same, therefore nothing was swapped...");
                }
                else if (gradeOne < -1 || gradeTwo < -1)
                {
                    Console.Clear();
                    Console.WriteLine("\tYou entered a negative number, so nothing happened...");
                }
                else
                {
                    Console.Clear();
                }
            }
        }
        static void WipeSelectedClassesMarks(List<Courses> aCourses, List<Grades> aGrades, Grades defaultGrade, StringBuilder classGradesFile, string classFile, int classChoiceIndex)
        {
            bool bLoop = true;
            while (bLoop)
            {
                int intMenuChoice = GetValidInt("\tEnter a menu choice >> ");
                if (intMenuChoice > 0 && intMenuChoice <= aGrades.Count + 1)
                {
                    Console.Clear();
                    Console.WriteLine("\t{0}: {1:N2}/{2:N2} = {3:N2}%, weighted: {4:N2}%", aGrades[intMenuChoice - 1].GetAssesmentName(),
                                                                                aGrades[intMenuChoice - 1].GetUserMark(),
                                                                                aGrades[intMenuChoice - 1].GetMaxMark(),
                                                                                aGrades[intMenuChoice - 1].GetMarkPercentage(),
                                                                                aGrades[intMenuChoice - 1].GetWeightedMark());
                    try
                    {
                        bool bEmptyGrades = false;
                        char ynChoice = GetYN("\tReally delete this assessment? [y/n] ");
                        if (ynChoice == 'y')
                        {
                            if (aGrades.Count == 1)
                            {
                                bEmptyGrades = true;
                            }
                            try
                            {
                                aCourses[classChoiceIndex].SetTotalWeight(aGrades[intMenuChoice - 1].GetWeight() * -1);
                            }
                            catch
                            {
                                throw new OverflowException();
                            }
                            try
                            {
                                aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[intMenuChoice - 1].GetWeightedMark() * -1);
                            }
                            catch
                            {
                                aCourses[classChoiceIndex].SetTotalWeight(aGrades[intMenuChoice - 1].GetWeight());
                            }
                            aGrades.RemoveAt(intMenuChoice - 1);
                            if (bEmptyGrades)
                            {
                                aGrades.Clear();
                                aGrades.Add(defaultGrade);
                            }
                            WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                            WriteToCoursesFile(aCourses, classFile);
                            bLoop = false;
                            Console.Clear();
                        }
                        else
                        {
                            Console.Clear();
                            bLoop = false;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Console.WriteLine("\t{0}", e.Message);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\tInvalid menu choice... try again");
                }
            }
        }
        static void WipeAllSelectedClassesMarks(List<Courses> aCourses, List<Grades> aGrades, Grades defaultGrade, StringBuilder classGradesFile, string classFile, int classChoiceIndex)
        {
            bool bLoop = true;
            while (bLoop)
            {
                try
                {
                    char ynChoice = GetYN("\tAre you sure you want to delete all the grades from this course? [y/n] ");
                    if (ynChoice == 'y')
                    {
                        aGrades.Clear();
                        aGrades.Add(defaultGrade);
                        WriteToGradesFile(aGrades, classGradesFile.Replace(classGradesFile[classGradesFile.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                classGradesFile.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                        WriteToCoursesFile(aCourses, classFile);
                        bLoop = false;
                        Console.Clear();
                    }
                    else
                    {
                        bLoop = false;
                        Console.Clear();
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("\t{0}", e.Message);
                }
            }
        }
        // Menus
        static char GetCourseMenuChoice()
        {
            Console.WriteLine("\t{0,4}********* Class menu *********", "");
            Console.WriteLine("\t{0,4}1) Select a class to perform actions on", "");
            Console.WriteLine("\t{0,4}2) Initialize classes", "");
            Console.WriteLine("\t{0,4}3) Edit/overwrite the selected class name and class ID", "");
            Console.WriteLine("\t{0,4}4) Wipe the selected class from file", "");
            Console.WriteLine("\t{0,4}5) Proceed to the next menu (edit/add grades)", "");
            Console.WriteLine("\t{0,4}6) Change the order of the classes", "");
            Console.WriteLine("\t{0,4}7) Change max number of classes", "");
            Console.WriteLine("\t{0,4}~) Clear all classes for this semester", "");
            Console.WriteLine("\t{0,4}x) Exit program", "");
            return GetValidChar($"\n\tEnter a menu choice >> ");
        } // Courses menu
        static char GetGradesMenuChoice()
        {
            Console.WriteLine("\n\t{0,4}********* Grades Menu *********", "");
            Console.WriteLine("\t{0,4}1) Select a class to perform actions on", "");
            Console.WriteLine("\t{0,4}2) Add marks to the selected class", "");
            Console.WriteLine("\t{0,4}3) Display the averages and all grades of the selected class", "");
            Console.WriteLine("\t{0,4}4) Edit marks on the selected class", "");
            Console.WriteLine("\t{0,4}5) Go back to last menu (edit/add classes)", "");
            Console.WriteLine("\t{0,4}6) Change the order of the grades", "");
            Console.WriteLine("\t{0,4}7) Remove marks on the selected class", "");
            Console.WriteLine("\t{0,4}~) Wipe all marks off the selected class", "");
            Console.WriteLine("\t{0,4}x) Exit", "");
            return GetValidChar($"\n\tEnter a menu choice >> ");
        } // Grades menu
        static void DisplayClassMenu(List<Courses> aCourses)
        {
            Console.WriteLine();
            for (int i = 0; i < aCourses.Count; i++)
            {
                Console.WriteLine("\t{0,4}{1}) Weight: {2,6}/{3,-3} |  {4} - {5}  ", "", i + 1, aCourses[i].GetTotalWeightedMark(), aCourses[i].GetTotalWeight(), aCourses[i].GetClassID(), aCourses[i].GetClassName());
            }
        }
        static void DisplayGradesMenu(List<Courses> aCourses, List<Grades> aGrades, int classChoiceIndex)
        {
            Console.WriteLine("\n\tTotal weighted mark for {0} - {1}: {2:N2}/{3:N2}\n", aCourses[classChoiceIndex].GetClassID(),
                                                                                    aCourses[classChoiceIndex].GetClassName(),
                                                                                    aCourses[classChoiceIndex].GetTotalWeightedMark(),
                                                                                    aCourses[classChoiceIndex].GetTotalWeight());
            for (int i = 0; i < aGrades.Count; i++)
            {
                Console.WriteLine("\t{0,4}{1}) {2}", "", i + 1, aGrades[i].GetAssesmentName());
                Console.WriteLine("\t{0,7}{1,-6:N2}/{2,-6:N2} = {3,6:N2}% - weighted: {4:N2}/{5:N2}", "", aGrades[i].GetUserMark(), aGrades[i].GetMaxMark(), aGrades[i].GetMarkPercentage(),
                                                                                        aGrades[i].GetWeightedMark(), aGrades[i].GetWeight());
            }
        }
        static void DisplayGrades(List<Grades> aGrades, string prompt)
        {
            Console.WriteLine(prompt);
            int indexDisplay = 0;
            foreach (var item in aGrades)
            {
                indexDisplay++;
                Console.WriteLine("\t{0,4}{1}) {2}", "", indexDisplay, item.GetAssesmentName());
                Console.WriteLine("\t{0,7}{1,-6:N2}/{2,-6:N2} = {3,6:N2}%, weighted: {4:N2}/{5:N2}", "", item.GetUserMark(), item.GetMaxMark(), item.GetMarkPercentage(), item.GetWeightedMark(), item.GetWeight());
            }
        }
        // validation
        static string GetValidString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        } // closes getstring
        static char GetValidChar(string prompt)
        {
            char letter = ' ';
            bool bValid = false;
            while (!bValid) // bValid == false
            {
                try
                {
                    Console.Write(prompt);
                    letter = Convert.ToChar(Console.ReadLine().Trim());
                    bValid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\tInvalid input... try again...\n");
                }
            }
            return letter;
        } // closes GetChar
        static int GetValidInt(string prompt)
        {
            int value = 0;
            bool bValid = false;
            while (!bValid) // bValid == false
            {
                try
                {
                    Console.Write(prompt);
                    value = Convert.ToInt32(Console.ReadLine().Trim());
                    bValid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\tInvalid input... try again...");
                }
            }
            return value;
        } // closes GetInt
        static double GetValidDouble(string prompt)
        {
            double value = 0.0;
            try
            {
                Console.Write(prompt);
                value = Convert.ToDouble(Console.ReadLine().Trim());
            }
            catch (Exception e)
            {
                throw new FormatException("\tInvalid input format, try again...");
            }
            return value;
        } // closes GetDouble
        static char GetYN(string prompt)
        {
            char letter = ' ';
            bool bValid = true;
            while (bValid) // bValid == false
            {
                try
                {
                    Console.Write(prompt);
                    letter = Convert.ToChar(Console.ReadLine().ToLower().Trim());
                    if (letter != 'y' && letter != 'n')
                    {
                        Console.WriteLine("\tInvalid choice, try again...");
                    }
                    else
                    {
                        bValid = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\tInvalid choice, try again...");
                }
            }
            return letter;
        }
        // File I/O
        static void SetMaxClasses(List<Courses> aCourses, string maxClassFile, char menuChoice, ref int maxClasses, ref bool programLoop)
        {
            if (menuChoice == '6')
            {
                Console.Clear();
                Console.WriteLine("\tThe current max number of classes is {0}", maxClasses);
                int userInput = GetValidInt("\tEnter the new maximum number of classes >> ");
                if (userInput < aCourses.Count)
                {
                    Console.Clear();
                    Console.WriteLine("\tThe max number of classes must be higher than that...\n\tTry removing a class or clearing all classes\n");
                }
                else
                {
                    try
                    {
                        using (StreamWriter fOut = new StreamWriter(maxClassFile))
                        {
                            fOut.WriteLine("{0}", userInput);
                        }
                        maxClasses = userInput;
                        Console.Clear();
                        Console.WriteLine("\tMax classes set to: {0}", maxClasses);
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("\tSomething went wrong");
                    }
                }
            }
            else
            {
                Console.Clear();
                try
                {
                    using (StreamReader fIn = new StreamReader(maxClassFile))
                    {
                        maxClasses = Convert.ToInt32(fIn.ReadLine());
                        if (maxClasses <= 0)
                        {
                            // set default if file is empty
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    try
                    {
                        using (StreamWriter fOut = new StreamWriter(maxClassFile))
                        {
                            // sets default
                            maxClasses = 5;
                            fOut.WriteLine("5");
                            Console.Clear();
                            Console.WriteLine("\tMax classes set to: {0}", maxClasses);
                        }
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("\tCan't find the Semester Tracker Files folder...\n\tCheck the manual");
                        programLoop = false;
                    }
                }
            }
        }
        static void ReadFromCoursesFile(List<Courses> aCourses, string fileName)
        {
            try
            {
                using (StreamReader fIn = new StreamReader(fileName))
                {
                    aCourses.Clear();
                    string line = "";
                    line = fIn.ReadLine();
                    while (line != null)
                    {
                        string[] parsed = line.Split('|');
                        Courses item = new Courses(parsed[0],
                                                    parsed[1]);
                        item.SetTotalWeight(Convert.ToDouble(parsed[2]));
                        item.SetTotalWeightedMark(Convert.ToDouble(parsed[3]));
                        aCourses.Add(item);
                        line = fIn.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
        static void WriteToCoursesFile(List<Courses> aCourses, string fileName)
        {
            try
            {
                using (StreamWriter fOut = new StreamWriter(fileName))
                {
                    for (int index = 0; index < aCourses.Count; index++)
                    {
                        Courses item = aCourses[index];
                        fOut.WriteLine("{0}|{1}|{2}|{3:N2}", item.GetClassName(), item.GetClassID(), item.GetTotalWeight(), Math.Round(item.GetTotalWeightedMark(), 2));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t{0}", e.Message);
            }
        }
        static void ReadFromGradesFile(List<Grades> aGrades, string fileName)
        {
            try
            {
                using (StreamReader fIn = new StreamReader(fileName))
                {
                    aGrades.Clear();
                    string line = "";
                    line = fIn.ReadLine();
                    while (line != null)
                    {
                        string[] parsed = line.Split('|');
                        Grades item = new Grades(parsed[0],
                                                 Convert.ToDouble(parsed[1]),
                                                 Convert.ToDouble(parsed[2]));
                        item.SetUserMark(Convert.ToDouble(parsed[3]));

                        aGrades.Add(item);
                        line = fIn.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
        static void WriteToGradesFile(List<Grades> aGrades, string fileName)
        {
            try
            {
                using (StreamWriter fOut = new StreamWriter(fileName))
                {
                    for (int index = 0; index < aGrades.Count; index++)
                    {
                        Grades item = aGrades[index];
                        fOut.WriteLine("{0}|{1}|{2}|{3}", item.GetAssesmentName(),
                                                        item.GetMaxMark(),
                                                        item.GetWeight(),
                                                        item.GetUserMark());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t{0}", e.Message);
            }
        }
    }
}
