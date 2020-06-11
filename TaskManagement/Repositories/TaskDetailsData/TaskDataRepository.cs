﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Repositories.TaskDetailsData
{
    public class TaskDataRepository : BaseRepository<TaskDataEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataRepository"/> class.
        /// </summary>
        /// <param name="configuration">Represents the application configuration.</param>
        /// <param name="isFromAzureFunction">Flag to show if created from Azure Function.</param>
        public TaskDataRepository(IConfiguration configuration, bool isFromAzureFunction = false)
            : base(
                configuration,
                PartitionKeyNames.TaskDetailsDataTable.TableName,
                PartitionKeyNames.TaskDetailsDataTable.TaskDataPartition,
                isFromAzureFunction)
        {
        }

        public async Task<string> GetLastCreatedTaskID()
        {
            var allRows = await this.GetAllAsync(PartitionKeyNames.TaskDetailsDataTable.TableName);
            string LastCreatedTaskID = allRows.OrderByDescending(c => c.TaskName).FirstOrDefault().ToString();
            return LastCreatedTaskID;
        }

        public async Task<TaskDataEntity> GetTaskDetailsByTaskIDAsync(Guid taskId)
        {
            var allRows = await this.GetAllAsync(PartitionKeyNames.TaskDetailsDataTable.TableName);
            TaskDataEntity taskDataEntity = allRows.Where(c => c.TaskId == taskId).FirstOrDefault();
            return taskDataEntity;
        }

        public async Task<List<string>> GetAllTaskTDAsync()
        {
            var allRows = await this.GetAllAsync(PartitionKeyNames.TaskDetailsDataTable.TableName);
            List<string> allIds = allRows.Select(c=>c.TaskName).ToList();
            return allIds;
        }
    }
}