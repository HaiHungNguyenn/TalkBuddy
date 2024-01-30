﻿namespace TalkBuddy.DAL.Interfaces;

public interface IUnitOfWork
{
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}