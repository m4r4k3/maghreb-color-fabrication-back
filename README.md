# Maghreb Color Fabrication — Backend API

ASP.NET Core REST API built for a Moroccan manufacturing company 
to manage their fabrication workflow independently from Sage 100, 
while keeping data synchronized with Sage.

## The Problem
Sage 100's native fabrication workflow didn't fit the company's 
production process. This API provides a custom workflow layer 
that handles fabrication orders on its own terms, with Sage 100 
as the backend data source.

## Tech Stack
- ASP.NET Core / C#
- SQL Server
- Entity Framework Core
- REST API architecture

## Features
- Custom fabrication order management
- Production workflow tracking
- Sage 100 data synchronization
- Role-based access control

## Getting Started
1. Clone the repo
2. Update connection string in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Start: `dotnet run`
