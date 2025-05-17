USE fithub;

-- Exercise definition
CREATE TABLE Exercises (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CreatedOn DATETIME,
    ModifiedOn DATETIME,
    Name VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    Type INT NOT NULL,  -- Enum: 0, 1, 2, 3
    DifficultyLevel INT NOT NULL,  -- Enum: 0, 1, 2
    Instructions TEXT NOT NULL,
    Categories VARCHAR(255) NOT NULL,
    MuscleGroups JSON NOT NULL
);

-- WorkoutDay: Day 1, Day 2, etc.
CREATE TABLE WorkoutDays (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CreatedOn DATETIME,
    ModifiedOn DATETIME,
    Name VARCHAR(255) NOT NULL,  -- e.g., "Day 1"
);

-- WorkoutExercise: Link between Exercise and WorkoutDay
CREATE TABLE WorkoutExercises (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CreatedOn DATETIME,
    ModifiedOn DATETIME,
    WorkoutDayId INT NOT NULL,
    ExerciseId INT NOT NULL,
    Sets JSON NOT NULL,  -- Example: [3, 3, 3]
    Reps JSON NOT NULL,  -- Example: [10, 8, 6]
    RestDuration INT,    -- Optional override
    Notes TEXT,
    `Order` INT NOT NULL, -- Position in the workout list
    FOREIGN KEY (WorkoutDayId) REFERENCES WorkoutDays(Id) ON DELETE CASCADE,
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(Id) ON DELETE CASCADE
);

-- (Optional) Enum lookup table for ExerciseType
CREATE TABLE ExerciseTypeEnum (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    TypeName VARCHAR(255) NOT NULL
);

INSERT INTO ExerciseTypeEnum (TypeName) VALUES
('Cardio'),
('Strength'),
('Flexibility'),
('Balance');
