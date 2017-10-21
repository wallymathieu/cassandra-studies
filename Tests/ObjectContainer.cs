using System;
using System.Collections.Generic;
using Cassandra;
using Cassandra.Mapping;
using Core;
using Core.Commands;
using Core.Domain;

namespace Tests
{
    internal class ObjectContainer
    {
        private CommandHandler[] handlers;
        private readonly IRepository _repository;
        private readonly ISession session;

        public ObjectContainer()
        {
            MappingConfiguration.Global.Define<CassandraMappings>();
            var cluster = Cluster.Builder()
                                  .AddContactPoint("127.0.0.1")
                                  .WithDefaultKeyspace("tests")
                                  .Build();
            session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
            // Connect to the nodes using a keyspace
            //session = cluster.Connect("tests");
            _repository = new CassandraRepository(session);
            handlers = new CommandHandler[] {
                new RepositoryCommandHandler(_repository).Handle,
            };
        }

        public IRepository GetRepository()
        {
            return _repository;
        }

        public void HandleAll(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
                foreach (var handler in handlers)
                    handler(command);
        }
    }
}
