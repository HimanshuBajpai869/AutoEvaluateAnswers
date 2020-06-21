// <copyright file="Student.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace EvaluateMVCApp
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    /// <summary>
    /// Represents the Student Model.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Gets or sets the Roll Number of the student.
        /// </summary>
        [DisplayName("Roll Number")]
        [Required]
        public string RollNo { get; set; }

        /// <summary>
        /// Gets or sets the Student Name.
        /// </summary>
        [MaxLength(50)]
        [DisplayName("Student Name")]
        [Required]
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the class standard.
        /// </summary>
        [DisplayName("Class")]
        [Required]
        public int Standard { get; set; }

        /// <summary>
        /// Gets or sets the class Subject.
        /// </summary>
        [Required]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Answer Number.
        /// </summary>
        [DisplayName("Question")]
        [Required]
        public int Question { get; set; }

        /// <summary>
        /// Gets or sets the Upload Submission File.
        /// </summary>
        [DisplayName("Upload Submission File")]
        public int SubmissionFilePath { get; set; }

        /// <summary>
        /// Gets or sets the Image File Path.
        /// </summary>
        public HttpPostedFileBase ImageFile { get; set; }
    }
}