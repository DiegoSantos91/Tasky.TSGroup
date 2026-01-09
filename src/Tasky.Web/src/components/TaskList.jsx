import TaskItem from './TaskItem';
import './TaskList.css';

function TaskList({ tasks, onUpdateTask, onChangeStatus, onDeleteTask }) {
    if (tasks.length === 0) {
        return (
            <div className="empty-state">
                <div className="empty-icon">📭</div>
                <h3>No tasks yet</h3>
                <p>Create your first task to get started!</p>
            </div>
        );
    }

    return (
        <div className="task-list">
            {tasks.map(task => (
                <TaskItem
                    key={task.id}
                    task={task}
                    onUpdate={onUpdateTask}
                    onChangeStatus={onChangeStatus}
                    onDelete={onDeleteTask}
                />
            ))}
        </div>
    );
}

export default TaskList;
