-- Create the database
CREATE DATABASE IF NOT EXISTS CalculationHistory;

-- Use the database
USE CalculationHistory;

-- Create the CalculationExpressionRecord table
CREATE TABLE IF NOT EXISTS CalculationExpressionRecord (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Record NVARCHAR(MAX) NULL
);