package org.nam.microprofile.health;

import io.agroal.api.AgroalDataSource;
import io.quarkus.agroal.DataSource;
import org.eclipse.microprofile.health.HealthCheck;
import org.eclipse.microprofile.health.HealthCheckResponse;
import org.eclipse.microprofile.health.HealthCheckResponseBuilder;
import org.eclipse.microprofile.health.Liveness;

import javax.enterprise.context.ApplicationScoped;
import javax.inject.Inject;
import java.sql.SQLException;

@Liveness
@ApplicationScoped
public class BackupDatabaseHealthCheck implements HealthCheck {
    @Inject
    @DataSource("backup")
    AgroalDataSource defaultDataSource;

    @Override
    public HealthCheckResponse call() {
        HealthCheckResponseBuilder responseBuilder = HealthCheckResponse.named("Backup database connection health check");
        try {
            var con = defaultDataSource.getConnection();
            if (con.isClosed()) {
                responseBuilder.down();
            } else {
                responseBuilder.up();
            }
        } catch (SQLException throwables) {
            responseBuilder.down();
        }
        return responseBuilder.build();
    }
}
