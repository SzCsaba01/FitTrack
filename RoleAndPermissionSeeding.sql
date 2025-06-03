-- Roles
-- Roles
DECLARE @UserRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @AdminRoleId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Roles (Id, RoleName) VALUES
(@UserRoleId, 'User'),
(@AdminRoleId, 'Admin');

-- Permissions
DECLARE @Perms TABLE (Name NVARCHAR(100), Id UNIQUEIDENTIFIER);
INSERT INTO Permissions (Id, Name)
OUTPUT inserted.Name, inserted.Id INTO @Perms(Name, Id)
VALUES
-- Profile / weight
(NEWID(), 'user:view_profile'),
(NEWID(), 'user:edit_profile'),
(NEWID(), 'user:view_weight_entries'),
(NEWID(), 'user:log_weight'),

-- Admin user mgmt
(NEWID(), 'user:view'),
(NEWID(), 'user:create'),
(NEWID(), 'user:edit'),
(NEWID(), 'user:delete'),

-- Workout
(NEWID(), 'workout:create'),
(NEWID(), 'workout:view'),
(NEWID(), 'workout:edit'),
(NEWID(), 'workout:delete'),

-- Meal
(NEWID(), 'meal:create'),
(NEWID(), 'meal:view'),
(NEWID(), 'meal:edit'),
(NEWID(), 'meal:delete'),

-- Recipe
(NEWID(), 'recipe:view'),
(NEWID(), 'recipe:create'),
(NEWID(), 'recipe:edit'),
(NEWID(), 'recipe:delete'),

-- Food
(NEWID(), 'food:view'),
(NEWID(), 'food:create'),
(NEWID(), 'food:edit'),
(NEWID(), 'food:delete'),

-- Exercise
(NEWID(), 'exercise:view'),
(NEWID(), 'exercise:create'),
(NEWID(), 'exercise:edit'),
(NEWID(), 'exercise:delete');


-- Role-Permission mappings (sample)

-- User permissions
INSERT INTO RolePermissionMappings (RoleId, PermissionId)
SELECT @UserRoleId, Id FROM @Perms WHERE Name IN (
    'user:view_profile',
    'user:edit_profile',
    'user:view_weight_entries',
    'user:log_weight',

    'workout:create',
    'workout:view',
    'workout:edit',
    'workout:delete',

    'meal:create',
    'meal:view',
    'meal:edit',
    'meal:delete',

    'recipe:view',

    'exercise:view',

    'food:view'
);

-- Admin gets all permissions
INSERT INTO RolePermissionMappings (RoleId, PermissionId)
SELECT @AdminRoleId, Id FROM @Perms;
