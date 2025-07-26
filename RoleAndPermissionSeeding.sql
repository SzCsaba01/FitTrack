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

-- Admin user mgmt
(NEWID(), 'user:manage'),
(NEWID(), 'user:view'),
(NEWID(), 'user:create'),
(NEWID(), 'user:edit'),
(NEWID(), 'user:delete'),

-- Recipe
(NEWID(), 'recipe:manage'),
(NEWID(), 'recipe:create'),
(NEWID(), 'recipe:edit'),
(NEWID(), 'recipe:delete'),

-- Food
(NEWID(), 'food:manage'),
(NEWID(), 'food:create'),
(NEWID(), 'food:edit'),
(NEWID(), 'food:delete'),

-- Exercise
(NEWID(), 'exercise:manage'),
(NEWID(), 'exercise:create'),
(NEWID(), 'exercise:edit'),
(NEWID(), 'exercise:delete');

-- Role-Permission mappings (sample)

-- User permissions
-- INSERT INTO RolePermissionMappings (RoleId, PermissionId)
-- SELECT @UserRoleId, Id FROM @Perms WHERE Name IN ();

-- Admin gets all permissions
INSERT INTO RolePermissionMappings (RoleId, PermissionId)
SELECT @AdminRoleId, Id FROM @Perms;
