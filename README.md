# CSV Processing Console Application (C#)

## 📌 Overview
This project is a C# console application that reads a CSV file containing personal information and generates two structured output files:

1. A **name frequency report**
2. A **sorted address report**

The application demonstrates file handling, data parsing, LINQ-based transformations, error handling, and input validation.

It also supports **unit testing integration (recommended with xUnit or NUnit)**.

---

## 📂 Input Data Format (CSV)

The expected CSV structure is:

```csv
FirstName,LastName,Address,Phone
Matt,Brown,12 Acton St,123456
Heinrich,Jones,31 Clifton Rd,123456
Johnson,Smith,22 Jones Rd,123456
Tim,Johnson,15 King St,123456
