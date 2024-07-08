namespace Scripts.FileLoaders
{
	public enum LoadStatus
	{
		InProgress,
		Success,
		Cancel,
		DiskIsFull,
		NoAccess,
		InternalError,
		ConnectionError,
		UnknownError,
	}
}