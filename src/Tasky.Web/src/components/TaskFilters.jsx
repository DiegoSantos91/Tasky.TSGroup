import './TaskFilters.css';
import { TaskStatus } from '../services/taskApi';

function TaskFilters({ currentFilter, onFilterChange, taskCounts }) {
    const filters = [
        { value: 'all', label: 'All', count: taskCounts.all, emoji: '📋' },
        { value: String(TaskStatus.Pending), label: 'Pending', count: taskCounts.pending, emoji: '⏳' },
        { value: String(TaskStatus.InProgress), label: 'In Progress', count: taskCounts.inProgress, emoji: '🔄' },
        { value: String(TaskStatus.Completed), label: 'Completed', count: taskCounts.completed, emoji: '✅' }
    ];

    return (
        <div className="task-filters">
            {filters.map(filter => (
                <button
                    key={filter.value}
                    className={`filter-btn ${currentFilter === filter.value ? 'active' : ''}`}
                    onClick={() => onFilterChange(filter.value)}
                >
                    <span className="filter-emoji">{filter.emoji}</span>
                    <span className="filter-label">{filter.label}</span>
                    <span className="filter-count">{filter.count}</span>
                </button>
            ))}
        </div>
    );
}

export default TaskFilters;
