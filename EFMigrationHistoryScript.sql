-- Drop the existing __EFMigrationsHistory table if it exists
DROP TABLE IF EXISTS "__EFMigrationsHistory";

-- Create the __EFMigrationsHistory table
CREATE TABLE "__EFMigrationsHistory" (
    "MigrationId" VARCHAR(255) NOT NULL PRIMARY KEY,
    "ProductVersion" VARCHAR(32) NOT NULL
);