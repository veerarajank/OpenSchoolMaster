using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS247.Models.Courses
{
    public class SemesterClass
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String Courses { get; set; }
        public List<CheckBoxDetails> CoursesInfo { get; set; }
        public List<SemesterClass> SemesterInfo { get; set; }
    }
    public class CheckBoxDetails
    {
        public String Text { get; set; }
        public int Id { get; set; }
        public bool IsChecked { get; set; }
    }
    public class CoursesModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public CheckBoxDetails IsEnable { get; set; }
        public List<CheckBoxDetails> ExamFormat { get; set; }
        public String SelectedButton { get; set; }
    }
    public class CourseList
    {
        public List<CoursesModel> Course { get; set; }
    }
    public class CourseSubjects
    {
        public List<CoursesModel> Course { get; set; }
        public List<SubjectModel> Subjects { get; set; }
    }
    public class CourseSubject
    {
        public CoursesModel Course { get; set; }
        public SubjectModel Subjects { get; set; }
    }
    public class SubjectModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String FirstSub { get; set; }
        public String SecondSub { get; set; }
        public int Weekly { get; set; }
    }
    public class TeacherModel
    {
        public int Id;
        public String Name;
    }
    public class AllObjects
    {
        public List<SubAllObjects> Items { get; set; }
    }
    public class BatchClass
    {
        public int Id;
        public String Name;
        public TeacherModel Teacher;
        public String StartDate;
        public String EndDate;
    }
    public class SubAllObjects
    {
        public CoursesModel Course { get; set; }
        public List<SubjectModel> Subjects { get; set; }
        public List<SemesterClass> Semester { get; set; }
        public List<BatchClass> Batch { get; set; }
    }
    
    
}