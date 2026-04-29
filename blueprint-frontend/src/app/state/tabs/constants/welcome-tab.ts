import { Tab } from '../models/tab';

const WELCOME_CODE = `
api TaskManagement
{
    uses outbox;
    uses cqrs;
    basepath "api/v1";
}

enum TaskStatus
{ 
    Todo,
    InProgress,
    Complete
}

value TaskInfo
{
    required text Title max(200);
    text Description max(1000);
}

aggregate Task
{
    key guid Id;
    required TaskInfo TaskInfo;
    required DateTime CreatedDate;
    date DueDate;
    required TaskStatus Status;
    
    Start()
        Set Status To InProgress
        Requires Status EqualsTo ToDo
        ErrorMessage "A task in progress or complete can't be set in progress";
    
    Complete()
        Set Status To Complete
        Requires Status EqualsTo InProgress
        ErrorMessage default;
    
    UpdateInfo(TaskInfo taskInfo)
        Set TaskInfo To taskInfo
        Requires Status DifferentThan Complete
        ErrorMessage "A completed task can't update basic info";

    Create(TaskInfo taskInfo, date dueDate)
        Set Id To default
        Set CreatedDate To now
        Set Status To ToDo
        Set TaskInfo To taskInfo
        Set DueDate To dueDate;
}

dto TaskDto maps Task
{
    guid Id maps Task.Id;
    text Title maps Task.TaskInfo.Title;
    text Description maps Task.TaskInfo.Description;
    date DueDate maps Task.DueDate;
    Status maps Task.Status;
}


controller Tasks at "tasks"
{
    post input TaskDto use Task.Create output TaskDto;
    get at "{key}" input guid output TaskDto;
    get at "get-all" output list of TaskDto;
    put at "{key}/complete" input guid output TaskDto use Task.Complete;
    put at "{key}/start" input guid output TaskDto use Task.Start;
    put at "{key}/update-info" input (guid, TaskDto) use Task.UpdateInfo output TaskDto;
    put at "{hey}/updateKey?apiKey" input UpdateTask output TaskDto;
}

command UpdateTask emit TaskDto
{
    required TaskInfo Info;
    required text ApiKey;
    required guid TaskId;
}

handler MyMessageHandler on UpdateTask
{
    invoke MyClient.EnsureAccess command.ApiKey;
    fetch Task command.TaskId
    with
    {
        invoke UpdateInfo command.Info;
        invoke Complete;
    }
}

handler MyOtherHandler on UpdateTask
{
    invoke MyClient.EnsureAccess command.ApiKey;
    create Task
    {
        command.Info;
        now;
    }
    with
    {
        Complete;
    }
}

apiclient MyClient
{
    EnsureAccess input text ApiKey;
}
`;

export const WELCOME_TAB: Tab = {
  id: 'home',
  title: 'main.ts',
  content: WELCOME_CODE,
  language: 'blueprint',
  isDirty: false,
};
