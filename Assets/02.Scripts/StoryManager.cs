using System.Collections.Generic;

public class StoryManager
{
    private readonly List<StoryNode> _stories;
    private int _currentStoryIndex;

    public StoryNode CurrentStory => _stories[_currentStoryIndex];

    public StoryManager(List<StoryNode> stories)
    {
        _stories = stories;
        _currentStoryIndex = 0;
    }

    public bool MoveToNextStory()
    {
        if (_currentStoryIndex + 1 < _stories.Count)
        {
            _currentStoryIndex++;
            return true;
        }
        return false;
    }
}
