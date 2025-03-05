USE fithub;
CREATE TABLE Exercises (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CreatedOn DATETIME,
    ModifiedOn DATETIME,
    Name VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Type INT NOT NULL,  -- Store number instead of string (0, 1, 2, 3)
    DifficultyLevel INT NOT NULL,  -- Store number (0, 1, 2)
    Sets JSON NOT NULL,
    Reps JSON NOT NULL,
    Instructions TEXT NOT NULL,
    Categories VARCHAR(255) NOT NULL,
    DurationOfRest INT NOT NULL,
    Notes TEXT,
    MuscleGroups JSON NOT NULL
);

-- You can then create a lookup table for the enums if you prefer
CREATE TABLE ExerciseTypeEnum (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    TypeName VARCHAR(255) NOT NULL
);

-- Insert possible values
INSERT INTO ExerciseTypeEnum (TypeName) VALUES
('Cardio'),
('Strength'),
('Flexibility'),
('Balance');
