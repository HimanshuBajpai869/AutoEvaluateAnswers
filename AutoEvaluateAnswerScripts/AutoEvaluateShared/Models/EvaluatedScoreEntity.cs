// <copyright file="EvaluatedScoreEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Represents Evaluated Score Entity.
    /// </summary>
    public class EvaluatedScoreEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluatedScoreEntity"/> class.
        /// </summary>
        public EvaluatedScoreEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluatedScoreEntity"/> class.
        /// </summary>
        /// <param name="rollNumber">Roll Number.</param>
        /// <param name="answerNumber">Answer Number.</param>
        public EvaluatedScoreEntity(string rollNumber, string answerNumber)
        {
            this.RowKey = rollNumber;
            this.PartitionKey = answerNumber;
        }

        /// <summary>
        /// Gets or sets the Roll Number of the student.
        /// </summary>
        public string RollNo { get; set; }

        /// <summary>
        /// Gets or sets the name of the student.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the Standard of the student.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the Subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Answer Number.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the Maximum Marks.
        /// </summary>
        public double MaximumMarks { get; set; } = 5.0;

        /// <summary>
        /// Gets or sets the Evaluated Score.
        /// </summary>
        public string EvaluatedScore { get; set; }
    }
}
