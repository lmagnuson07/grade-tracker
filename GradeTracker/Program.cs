using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PreMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder classData = new StringBuilder("C:\\Users\\logma\\OneDrive - NAIT\\Documents\\Semester Tracker Files\\Marks-List\\Grades-Course-1.txt");
            const string CLASS_FILE_NAME = "C:\\Users\\logma\\OneDrive - NAIT\\Documents\\Semester Tracker Files\\Course-Names.txt";
            const string MAX_CLASS_FILE = "C:\\Users\\logma\\OneDrive - NAIT\\Documents\\Semester Tracker Files\\Max-Class.txt";
            bool programLoop = true;
            char ynChoice = ' ';
            int maxClasses = 0, intTemp = 0;
            ///////////////////////////////////////////////////////////////////////////////////////////////
            // Gets the max class value from file
            try
            {
                using (StreamReader fIn = new StreamReader(MAX_CLASS_FILE))
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
                    using (StreamWriter fOut = new StreamWriter(MAX_CLASS_FILE))
                    {
                        // sets default
                        maxClasses = 5;
                        fOut.WriteLine("5");
                    }
                }
                catch
                {
                    Console.WriteLine("\tCan't find the Semester Tracker Files folder...\n\tCheck the manual");
                    programLoop = false;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////
            List<Courses> aCourses = new List<Courses>();
            List<Grades> aGrades = new List<Grades>(aCourses.Count);
            List<Grades> aGradesTemp = new List<Grades>();
            Grades defaultGrade = new Grades("/~~/~/~///~/~/", 0, 0);
            // read from the classes file to determine nClasses
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
            // read from class data to populate class (if empty populate with default values)
            int intMenuChoice = 0, classChoiceIndex = -1;
            char menuChoice = ' ';
            string className = "", classID = "";
            bool bLoop = true, bInnerLoop = true, bIndexSelected = false;
            while (programLoop)
            {
                bLoop = true;
                if (aCourses.Count < 1)
                {
                    while (aCourses.Count < maxClasses && bLoop)
                    {
                        try
                        {
                            Console.WriteLine("\n\t************* Semester Grade Tracker *************");
                            Console.WriteLine("\t******** Lets initialize those classes!! *********");
                            Console.WriteLine();
                            className = GetValidString($"\tEnter the class name >> ");
                            classID = GetValidString($"\tEnter the class ID >> ");
                            Courses course = new Courses(className, classID);
                            aCourses.Add(course);
                            aGrades.Add(defaultGrade);
                            WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), aCourses.Count.ToString(),
                                classData.Length - 5, aCourses.Count < 10 ? 1 : 2).ToString());
                            aGrades.Clear();
                            ynChoice = GetYN("\n\tDo you want to enter another class? [y/n] ");
                            if (ynChoice == 'n')
                            {
                                bLoop = false;
                            }
                            Console.Clear();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("\t{0}", e.Message);
                        }
                    }
                    WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                    Console.Clear();
                }
                Console.WriteLine("\n\t************* Semester Grade Tracker *************");

                bLoop = true;
                while (bLoop)
                {
                    Console.WriteLine(classChoiceIndex > -1 && bIndexSelected ? $"\tSelected class: {aCourses[classChoiceIndex].GetClassID()} - {aCourses[classChoiceIndex].GetClassName()} | " +
                        $"Weight: {aCourses[classChoiceIndex].GetTotalWeightedMark():N2}/{aCourses[classChoiceIndex].GetTotalWeight():N2}\n" : "");
                    Console.WriteLine("\tChoose a class, or edit one. Your choice...");
                    menuChoice = GetCourseMenuChoice();

                    switch (menuChoice)
                    {
                        case '1': // Choose a class
                            {
                                if (aCourses.Count < 1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't initialized any classes yet...");
                                }
                                else
                                {
                                    // display a menu of the available classes. Display using a for loop
                                    bInnerLoop = true;
                                    while (bInnerLoop)
                                    {
                                        PrintClassMenu(aCourses);
                                        intMenuChoice = GetValidInt($"\n\t{"",4}Choose a class bro >> ");
                                        if (intMenuChoice > 0 && intMenuChoice <= aCourses.Count)
                                        {
                                            classChoiceIndex = intMenuChoice - 1;
                                            ReadFromGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), intMenuChoice.ToString(),
                                                        classData.Length - 5, intMenuChoice < 10 ? 1 : 2).ToString());
                                            bIndexSelected = true;
                                            bInnerLoop = false;
                                            Console.Clear();
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nInvalid menu choice, try again...");
                                        }
                                    }
                                }
                                break;
                            }
                        case '2': // Initialize classes
                            {
                                bInnerLoop = true;
                                while (aCourses.Count < maxClasses && bInnerLoop)
                                {
                                    try
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n\tYou are at {1} of {2} classes", "", aCourses.Count, maxClasses);
                                        className = GetValidString("\tEnter the class name >> ");
                                        classID = GetValidString("\tEnter the class ID >> ");
                                        Courses course = new Courses(className, classID);
                                        aCourses.Add(course);
                                        aGradesTemp.Add(defaultGrade);
                                        WriteToGradesFile(aGradesTemp, classData.Replace(classData[classData.Length - 5].ToString(), aCourses.Count.ToString(),
                                            classData.Length - 5, aCourses.Count < 10 ? 1 : 2).ToString());
                                        aGradesTemp.Clear();
                                        ynChoice = GetYN("\tDo you want to enter another class? [y/n] ");
                                        if (ynChoice == 'n')
                                        {
                                            bInnerLoop = false;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("\t{0}", e.Message);
                                    }
                                }
                                if (aCourses.Count >= maxClasses)
                                {
                                    Console.WriteLine("\tYou've entered the maximum number of classes...\n\tWipe the semester or delete a class.");
                                }
                                WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                Console.Clear();
                                break;
                            }
                        case '3': // edit/overwrite class name/ID
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class yet my dude");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou are about to overwrite {0} - {1}", aCourses[classChoiceIndex].GetClassID(), aCourses[classChoiceIndex].GetClassName());
                                    // ask to continue
                                    ynChoice = GetYN("\tContinue? [y/n] ");
                                    if (ynChoice == 'y')
                                    {
                                        bInnerLoop = true;
                                        while (bInnerLoop)
                                        {
                                            className = GetValidString("\tEnter the new class name >> ");
                                            classID = GetValidString("\tEnter the new class ID >> ");
                                            try
                                            {
                                                Courses course = new Courses(className, classID);
                                                bInnerLoop = false;
                                                // add object back to list and write to file
                                                aCourses[classChoiceIndex] = course;
                                                WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                                Console.Clear();
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine("\t{0}", e.Message);
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case '4': // wipe the class from file
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class yet my dude");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou are about to remove {0} - {1}", aCourses[classChoiceIndex].GetClassID(), aCourses[classChoiceIndex].GetClassName());
                                    bInnerLoop = true;
                                    while (bInnerLoop)
                                    {
                                        try
                                        {
                                            ynChoice = GetYN("\tAre you sure you want to remove this class? [y/n] ");
                                            if (ynChoice == 'n')
                                            {
                                                bInnerLoop = false;
                                            }
                                            else
                                            {
                                                aCourses.RemoveAt(classChoiceIndex);
                                                bIndexSelected = false;
                                                bInnerLoop = false;
                                                // write new array to class
                                                WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                                // overwrite the deleted class grade file with nothing.
                                                aGrades.Clear();
                                                // clears the selected classes grades
                                                WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), intMenuChoice.ToString(),
                                                    classData.Length - 5, intMenuChoice + 1 < 10 ? 1 : 2).ToString());
                                                // Shifts data on the files over 
                                                for (int i = classChoiceIndex; i < aCourses.Count - 1; i++)
                                                {
                                                    ReadFromGradesFile(aGradesTemp, classData.Replace(classData[classData.Length - 5].ToString(), (i + 2).ToString(),
                                                        classData.Length - 5, i + 2 < 10 ? 1 : 2).ToString());
                                                    WriteToGradesFile(aGradesTemp, classData.Replace(classData[classData.Length - 5].ToString(), (i + 1).ToString(),
                                                        classData.Length - 5, i + 1 < 10 ? 1 : 2).ToString());
                                                }
                                                // after everything has been shifted, make the last file empty
                                                aGradesTemp.Clear();
                                                WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), (aCourses.Count + 1).ToString(),
                                                    classData.Length - 5, aCourses.Count + 1 < 10 ? 1 : 2).ToString());
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
                                break;
                            }
                        case '5': // proceed to next menu
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class yet my dude");
                                }
                                else
                                {
                                    bLoop = false;
                                    Console.Clear();
                                }
                                break;
                            }
                        case '6': // change max classes
                            {
                                Console.Clear();
                                Console.WriteLine("\tThe current max number of classes is {0}", maxClasses);
                                intTemp = GetValidInt("\tEnter the new maximum number of classes >> ");
                                Console.Clear();
                                if (intTemp < aCourses.Count)
                                {
                                    Console.WriteLine("\tThe max number of classes must be higher than that...\n\tTry removing a class or clearing all classes\n");
                                }
                                else
                                {
                                    try
                                    {
                                        using (StreamWriter fOut = new StreamWriter(MAX_CLASS_FILE))
                                        {
                                            fOut.WriteLine("{0}", intTemp);
                                        }
                                        maxClasses = intTemp;
                                        Console.WriteLine("\tMax classes set to: {0}", maxClasses);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\tSomething went wrong");
                                    }
                                }
                                break;
                            }
                        case '~': // clear all classes for semester
                            {
                                if (classChoiceIndex == -1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tYou haven't chosen a class yet my dude");
                                }
                                else
                                {
                                    Console.WriteLine();
                                    bInnerLoop = true;
                                    while (bInnerLoop)
                                    {
                                        Console.Clear();
                                        ynChoice = GetYN("\tAre you sure you want to remove all the classes from this semester?\n\tOnce you do this, you can't revert back [y/n] ");
                                        if (ynChoice == 'n')
                                        {
                                            bInnerLoop = false;
                                        }
                                        else
                                        {
                                            bInnerLoop = false;
                                            bIndexSelected = false;
                                            // write new empty array to file
                                            aGrades.Clear();
                                            int temp = 0;
                                            foreach (var item in aCourses)
                                            {
                                                // wipes each grades file.
                                                temp++;
                                                WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), temp.ToString(),
                                                    classData.Length - 5, temp < 10 ? 1 : 2).ToString().ToString());
                                            }
                                            aCourses.Clear();
                                            WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                            Console.Clear();
                                        }
                                    }
                                }
                                break;
                            }
                        case 'x': // exit program
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
                string assessmentName = "";
                double maxMark = 0, userGrade = 0, weight = 0;
                int indexDisplay = 0, innerIntMenuChoice = 0;
                bool bEmptyGrades = false;
                while (menuChoice != 'x' && programLoop)
                {
                    Console.WriteLine("\n\t************* Semester Grade Tracker *************");
                    Console.WriteLine($"\tSelected class: {aCourses[classChoiceIndex].GetClassID()} - {aCourses[classChoiceIndex].GetClassName()} | " +
                        $"Weight: {aCourses[classChoiceIndex].GetTotalWeightedMark():N2}/{aCourses[classChoiceIndex].GetTotalWeight():N2}");
                    menuChoice = GetMenuChoice();
                    switch (menuChoice)
                    {
                        case '1': // select class to edit
                            {
                                Console.WriteLine("\t************ Select a class *****************");
                                bInnerLoop = true;
                                while (bInnerLoop)
                                {
                                    PrintClassMenu(aCourses);
                                    intMenuChoice = GetValidInt($"\n\t{"",4}Choose a class bro >> ");
                                    if (intMenuChoice > 0 && intMenuChoice <= aCourses.Count)
                                    {
                                        classChoiceIndex = intMenuChoice - 1;
                                        ReadFromGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), intMenuChoice.ToString(),
                                                        classData.Length - 5, intMenuChoice < 10 ? 1 : 2).ToString());
                                        bInnerLoop = false;
                                        Console.Clear();
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nInvalid menu choice, try again...");
                                    }
                                }
                                break;
                            }
                        case '2': // add a to selected class
                            {
                                // Prompt use to enter max mark, my mark, and weight, than ask if input is correct before saving. 
                                assessmentName = "";
                                maxMark = 0; userGrade = 0; weight = 0;
                                bInnerLoop = true;
                                while (bInnerLoop && aCourses[classChoiceIndex].GetTotalWeight() < 100)
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
                                        ynChoice = GetYN("\tContinue to save? [y/n] ");
                                        if (ynChoice == 'y')
                                        {
                                            // clears array if the default values are given
                                            if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                                            {
                                                aGrades.Clear();
                                            }
                                            Grades grade = new Grades(assessmentName, maxMark, weight);
                                            grade.SetUserMark(userGrade);
                                            aGrades.Add(grade);
                                            aCourses[classChoiceIndex].SetTotalWeight(weight);
                                            aCourses[classChoiceIndex].SetTotalWeightedMark(grade.GetWeightedMark());
                                            WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                                    classData.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                                            WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                            bInnerLoop = false;
                                            Console.Clear();
                                        }
                                        else
                                        {
                                            bInnerLoop = false;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        aGrades.RemoveAt(aGrades.Count - 1);
                                        Console.Clear();
                                        Console.WriteLine("\t{0}", e.Message);
                                    }
                                }
                                if (aCourses[classChoiceIndex].GetTotalWeight() >= 100)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tThe total weight is already 100, and cannot be higher...");
                                }
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
                                    Console.WriteLine("\tTotal weighted mark for {0} - {1}: {2:N2}%", aCourses[classChoiceIndex].GetClassID(),
                                                                                    aCourses[classChoiceIndex].GetClassName(),
                                                                                    aCourses[classChoiceIndex].GetTotalWeightedMark());
                                    foreach (var item in aGrades)
                                    {
                                        Console.WriteLine("\t{0}", item.GetAssesmentName());
                                        Console.WriteLine("\t{0:N2}/{1:N2} = {2:N2}% - weighted: {3:N2}%", item.GetUserMark(), item.GetMaxMark(), item.GetMarkPercentage(), item.GetWeightedMark());
                                    }
                                    Console.WriteLine("\tPress enter to continue...");
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
                                    Console.WriteLine("\tChoose a mark to edit...");
                                    indexDisplay = 0;
                                    foreach (var item in aGrades)
                                    {
                                        indexDisplay++;
                                        Console.WriteLine("\t{0}) {1}", indexDisplay, item.GetAssesmentName());
                                        Console.WriteLine("\t{0,3}{1:N2}/{2:N2} = {3:N2}%, weighted: {4:N2}%", "", item.GetUserMark(), item.GetMaxMark(), item.GetMarkPercentage(), item.GetWeightedMark());
                                    }
                                    bInnerLoop = true;
                                    while (bInnerLoop)
                                    {
                                        innerIntMenuChoice = GetValidInt("\tEnter a menu choice >> ");
                                        if (innerIntMenuChoice > 0 && innerIntMenuChoice <= indexDisplay)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\t{0}: {1:N2}/{2:N2} = {3:N2}%, weighted: {4:N2}% ({5:N2}% weight)", aGrades[innerIntMenuChoice - 1].GetAssesmentName(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetUserMark(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetMaxMark(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetMarkPercentage(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetWeightedMark(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetWeight());
                                            assessmentName = GetValidString("\tEnter the new assessment name >> ");
                                            maxMark = GetValidDouble("\tEnter the new maximum mark >> ");
                                            weight = GetValidDouble("\tEnter the new weight >> ");
                                            userGrade = GetValidDouble("\tEnter the new mark >> ");
                                            try
                                            {
                                                Console.Clear();
                                                Console.WriteLine("\t{0}: {1:N2}/{2:N2} - Weight: {3:N2}", assessmentName, userGrade, maxMark, weight);
                                                ynChoice = GetYN("\tContinue to save? [y/n] ");
                                                if (ynChoice == 'y')
                                                {
                                                    Grades grade = new Grades(assessmentName, maxMark, weight);
                                                    grade.SetUserMark(userGrade);
                                                    aCourses[classChoiceIndex].SetTotalWeight(aGrades[innerIntMenuChoice - 1].GetWeight() * -1);
                                                    aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[innerIntMenuChoice - 1].GetWeightedMark() * -1);
                                                    aGrades[innerIntMenuChoice - 1] = grade;
                                                    aCourses[classChoiceIndex].SetTotalWeight(weight);
                                                    aCourses[classChoiceIndex].SetTotalWeightedMark(grade.GetWeightedMark());
                                                    WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                                        classData.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                                                    WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                                    bInnerLoop = false;
                                                    Console.Clear();
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    bInnerLoop = false;
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
                                break;
                            }
                        case '5': // Go back to last menu
                            {
                                menuChoice = 'x';
                                programLoop = true;
                                Console.Clear();
                                break;
                            }
                        case '6': // Remove selected classes marks 
                            {
                                if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tThere are no marks on the current class yet...");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tChoose a mark to remove...");
                                    indexDisplay = 0;
                                    foreach (var item in aGrades)
                                    {
                                        indexDisplay++;
                                        Console.WriteLine("\t{0}) {1}", indexDisplay, item.GetAssesmentName());
                                        Console.WriteLine("\t{0,4}{1:N2}/{2:N2} = {3:N2}%, weighted: {4:N2}%", "", item.GetUserMark(), item.GetMaxMark(), item.GetMarkPercentage(), item.GetWeightedMark());
                                    }
                                    bInnerLoop = true;
                                    while (bInnerLoop)
                                    {
                                        innerIntMenuChoice = GetValidInt("\tEnter a menu choice >> ");
                                        if (innerIntMenuChoice > 0 && innerIntMenuChoice <= indexDisplay)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("\t{0}: {1:N2}/{2:N2} = {3:N2}%, weighted: {4:N2}%", aGrades[innerIntMenuChoice - 1].GetAssesmentName(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetUserMark(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetMaxMark(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetMarkPercentage(),
                                                                                                        aGrades[innerIntMenuChoice - 1].GetWeightedMark());
                                            try
                                            {
                                                bEmptyGrades = false;
                                                ynChoice = GetYN("\tReally delete this assessment? [y/n] ");
                                                if (ynChoice == 'y')
                                                {
                                                    if (aGrades.Count == 1)
                                                    {
                                                        bEmptyGrades = true;
                                                    }
                                                    aCourses[classChoiceIndex].SetTotalWeight(aGrades[innerIntMenuChoice - 1].GetWeight() * -1);
                                                    aCourses[classChoiceIndex].SetTotalWeightedMark(aGrades[innerIntMenuChoice - 1].GetWeightedMark() * -1);
                                                    aGrades.RemoveAt(innerIntMenuChoice - 1);
                                                    if (bEmptyGrades)
                                                    {
                                                        aGrades.Clear();
                                                        aGrades.Add(defaultGrade);
                                                    }
                                                    WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                                        classData.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                                                    WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                                    bInnerLoop = false;
                                                    Console.Clear();
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    bInnerLoop = false;
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
                                break;
                            }
                        case '7': // Wipe selected classes marks
                            {
                                if (aGrades[0].GetAssesmentName() == "/~~/~/~///~/~/")
                                {
                                    Console.Clear();
                                    Console.WriteLine("\tThere are no marks on the current class yet...");
                                }
                                else
                                {
                                    Console.Clear();
                                    indexDisplay = 0;
                                    foreach (var item in aGrades)
                                    {
                                        indexDisplay++;
                                        Console.WriteLine("\t{0}) {1}", indexDisplay, item.GetAssesmentName());
                                        Console.WriteLine("\t{0,4}{1:N2}/{2:N2} = {3:N2}%, weighted: {4:N2}%", "", item.GetUserMark(), item.GetMaxMark(), item.GetMarkPercentage(), item.GetWeightedMark());
                                    }
                                    bInnerLoop = true;
                                    while (bInnerLoop)
                                    {
                                        try
                                        {
                                            ynChoice = GetYN("\tAre you sure you want to delete all the grades from this course? [y/n] ");
                                            if (ynChoice == 'y')
                                            {
                                                aGrades.Clear();
                                                aGrades.Add(defaultGrade);
                                                WriteToGradesFile(aGrades, classData.Replace(classData[classData.Length - 5].ToString(), (classChoiceIndex + 1).ToString(),
                                                        classData.Length - 5, classChoiceIndex + 1 < 10 ? 1 : 2).ToString().ToString());
                                                WriteToCoursesFile(aCourses, CLASS_FILE_NAME);
                                                bInnerLoop = false;
                                                Console.Clear();
                                            }
                                            else
                                            {
                                                bInnerLoop = false;
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
                                break;
                            }
                        case 'x': // exit program
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
            Console.WriteLine("bye");
            Console.ReadLine();
            Console.WriteLine("\tPress enter to exit");
        }
        static void PrintClassMenu(List<Courses> aCourses)
        {
            Console.Clear();
            Console.WriteLine();
            for (int i = 0; i < aCourses.Count; i++)
            {
                Console.WriteLine("\t{0,4}{1}) {2} - {3}", "", i + 1, aCourses[i].GetClassID(), aCourses[i].GetClassName());
            }
        }
        static char GetCourseMenuChoice()
        {
            Console.WriteLine("\t{0,4}1) Select a class to do stuff to", "");
            Console.WriteLine("\t{0,4}2) Initialize classes", "");
            Console.WriteLine("\t{0,4}3) Edit/overwrite the selected class name and class ID", "");
            Console.WriteLine("\t{0,4}4) Wipe the selected class from file", "");
            Console.WriteLine("\t{0,4}5) Proceed to the next menu", "");
            Console.WriteLine("\t{0,4}6) Change max number of classes", "");
            Console.WriteLine("\t{0,4}~) Clear all classes for this semester", "");
            Console.WriteLine("\t{0,4}x) Exit program", "");
            return GetValidChar($"\n\t{"",4}Enter a menu choice >> ");
        }
        static char GetMenuChoice()
        {
            Console.WriteLine("\n\t{0,4}********* Dope Ass Menu *********", "");
            Console.WriteLine("\t{0,4}1) Select a class to edit", "");
            Console.WriteLine("\t{0,4}2) Add marks to the selected class", "");
            Console.WriteLine("\t{0,4}3) Display the averages and all grades of the selected class", "");
            Console.WriteLine("\t{0,4}4) Edit marks on the selected class", "");
            Console.WriteLine("\t{0,4}5) Go back to last menu (edit classes)", "");
            Console.WriteLine("\t{0,4}6) Remove marks on the selected class", "");
            Console.WriteLine("\t{0,4}7) Wipe all marks off the selected class", "");
            Console.WriteLine("\t{0,4}x) Exit", "");
            return GetValidChar($"\n\t{"",4}Enter a menu choice >> ");
        } // closses GetMenuChoice
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
                    letter = Convert.ToChar(Console.ReadLine());
                    bValid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\tInvalid input... try again.\n");
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
                    value = Convert.ToInt32(Console.ReadLine());
                    bValid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid input... try again.\n");
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
                value = Convert.ToDouble(Console.ReadLine());
            }
            catch (Exception e)
            {
                throw new Exception("Invalid input format, try again...");
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
                    letter = Convert.ToChar(Console.ReadLine().ToLower());
                    if (letter != 'y' && letter != 'n')
                    {
                        Console.WriteLine("\tInvalid choice... try again");
                    }
                    else
                    {
                        bValid = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\tInvalid choice... try again");
                }
            }
            return letter;
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
                        fOut.WriteLine("{0}|{1}|{2}|{3}", item.GetClassName(), item.GetClassID(), item.GetTotalWeight(), item.GetTotalWeightedMark());
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

