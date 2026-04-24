import { Tab } from '../models/tab';

const WELCOME_CODE = `// Welcome to Blueprint
// Click "Home" in the sidebar to open this tab

// Task management API definition

resource Task
{
    key Id Guid

    required Title string max(200)
    Description string max(1000)

    // TODO: enforce enum — only Todo | InProgress | Done
    // TODO: default value must be set to Todo at application layer
    Status string max(20)

    CreatedAt string
    DueDate string

    rules
    {
        Title notEmpty
        Status notEmpty
    }
}

// DTO for creating a new task
request CreateTaskDto (Task t) =>
{
    Title,
    Description,
    DueDate
}

// DTO for updating task fields
request UpdateTaskDto (Task t) =>
{
    Title,
    Description,
    DueDate
}

response TaskSummaryDto (Task t) =>
{
    Id,
    Title,
    Status,
    DueDate
}

response TaskDetailDto (Task t) =>
{
    Id,
    Title,
    Description,
    Status,
    CreatedAt,
    DueDate,
    Label => $"{Title} [{Status}]"
}

controller Tasks at "/api/tasks"
{
    post "/"
    {
        input CreateTaskDto
        output TaskDetailDto
    }

    get "/"
    {
        output TaskSummaryDto
    }

    get "/{id}"
    {
        input id
        output TaskDetailDto
    }

    // TODO: validate transition Todo → InProgress at application layer
    put "/{id}/start"
    {
        input id
        output TaskDetailDto
    }

    // TODO: validate transition InProgress → Done at application layer
    put "/{id}/done"
    {
        input id
        output TaskDetailDto
    }
}`;

export const WELCOME_TAB: Tab = {
  id: 'home',
  title: 'main.ts',
  content: WELCOME_CODE,
  language: 'blueprint',
  isDirty: false,
};
