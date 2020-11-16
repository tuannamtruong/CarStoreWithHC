package org.nam.microprofile.health;

import io.agroal.api.AgroalDataSource;
import io.quarkus.agroal.DataSource;
import org.eclipse.microprofile.health.HealthCheck;
import org.eclipse.microprofile.health.HealthCheckResponse;
import org.eclipse.microprofile.health.HealthCheckResponseBuilder;
import org.eclipse.microprofile.health.Readiness;

import javax.enterprise.context.ApplicationScoped;
import javax.inject.Inject;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;

@Readiness
@ApplicationScoped
public class BackupDatabaseReadyCheck implements HealthCheck {

    @Inject
    @DataSource("backup")
    AgroalDataSource defaultDataSource;
    String queryCheckWaitfor = "Select \n" +
            "a.command,\n" +
            "s.database_id\n" +
            "FROM   \n" +
            "sys.dm_tran_locks l\n" +
            "JOIN sys.dm_exec_sessions s ON l.request_session_id = s.session_id\n" +
            "LEFT JOIN   \n" +
            "(\n" +
            " SELECT  *\n" +
            " FROM    sys.dm_exec_requests r\n" +
            " CROSS APPLY sys.dm_exec_sql_text(sql_handle)\n" +
            ") a ON s.session_id = a.session_id";

    @Override
    public HealthCheckResponse call() {
        HealthCheckResponseBuilder responseBuilder = HealthCheckResponse.named("Backup database readiness check");
        Connection con = null;
        Statement stmt = null;
        ResultSet rs = null;
        try {
            con = defaultDataSource.getConnection();
            stmt = con.createStatement();
            rs = stmt.executeQuery(queryCheckWaitfor);
            while (rs.next()) {
                String command = rs.getString("command");
                int database_id = rs.getInt("database_id");
                if (command != null
                        && command.equalsIgnoreCase("WAITFOR")
                        && database_id == 6) {
                    responseBuilder.down().withData("Reason: ", "Server is blocked.");
                    break;
                } else {
                    responseBuilder.up();
                }
            }
        } catch (Exception throwables) {
            responseBuilder.down();
        } finally {
            try {
                if (rs != null) rs.close();
            } catch (Exception e) {
                responseBuilder.withData("rs","cantclose");
            }
            try {
                if (stmt != null) stmt.close();
            } catch (Exception e) {
                responseBuilder.withData("stmt","cantclose");
            }
            try {
                if (con != null) con.close();
            } catch (Exception e) {
                responseBuilder.withData("con","cantclose");
            }
        }
        return responseBuilder.build();
    }
}
