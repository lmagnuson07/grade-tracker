using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreMethods
{
    class Grades
    {
        private string assessmentName;
        private double maxMark;
        private double userMark;
        private double weight;
        public Grades(string assessmentName, double markTotal, double weight)
        {
            if (assessmentName.Length > 50)
            {
                throw new Exception("The assessment name is to long... must be less than 50 characters");
            }
            else if (assessmentName.Length < 4)
            {
                throw new Exception("The assessment name must be at least 4 characters long");
            }
            else
            {
                this.assessmentName = assessmentName;
            }
            if (markTotal < 0)
            {
                throw new Exception("Mark total must be greater than 0");
            }
            else
            {
                this.maxMark = markTotal;
            }

            if (weight < 0)
            {
                throw new Exception("Weight must be greater than 0");
            }
            else
            {
                this.weight = weight;
            }
        }
        public double GetWeightedMark()
        {
            return Math.Round(GetMarkPercentage() * weight / 100, 2);
        }
        public double GetMarkPercentage()
        {
            return Math.Round((userMark / maxMark) * 100, 2);
        }
        public string GetAssesmentName()
        {
            return assessmentName;
        }
        public double GetMaxMark()
        {
            return maxMark;
        }
        public void SetUserMark(double userMark)
        {
            if (userMark < 0)
            {
                throw new Exception("User mark must be greater than 0");
            }
            else if (userMark > GetMaxMark())
            {
                throw new Exception("The user mark cannot exceed the max mark");
            }
            else
            {
                this.userMark = userMark;
            }
        }
        public double GetUserMark()
        {
            return userMark;
        }
        public double GetWeight()
        {
            return weight;
        }
    }
}

