using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterGradeTracker
{
    class Courses
    {
        private string className;
        private string classID;
        private double totalWeight;
        private double totalWeightedMark;
        public Courses(string className, string classID)
        {
            if (className.Length < 4)
            {
                throw new OverflowException("The class name cannot be shorter than 4 characters");
            }
            else if (className.Length > 50)
            {
                throw new OverflowException("The class name cannot be longer than 50 characters");
            }
            else
            {
                this.className = className;
            }
            if (classID.Length != 8)
            {
                throw new OverflowException("The class ID must be 8 characters long");
            }
            else
            {
                this.classID = classID;
            }
        }
        public double GetTotalWeightedMark()
        {
            return Math.Round(totalWeightedMark, 2);
        }
        public void SetTotalWeightedMark(double weightedMark)
        {
            if (totalWeightedMark + weightedMark < 0)
            {
                throw new OverflowException("This would result in a total weighted mark of less than 0. Check your files...");
            }
            else if (totalWeightedMark + weightedMark > 100)
            {
                throw new OverflowException("This would result in a total weighted mark greater than 100. Check your files...");
            }
            else
            {
                totalWeightedMark += weightedMark;
            }
        }
        public double GetTotalWeight()
        {
            return Math.Round(totalWeight, 2);
        }
        public void SetTotalWeight(double weight)
        {
            if (totalWeight + weight > 100)
            {
                throw new OverflowException("The weight entered would exceed a total weight of 100");
            }
            else if (totalWeight + weight < 0)
            {
                throw new OverflowException("This would result in a total weight of less than 0. Check your files...");
            }
            else
            {
                totalWeight += weight;
            }
        }
        public string GetClassName()
        {
            return className;
        }
        public string GetClassID()
        {
            return classID;
        }
    }
}
