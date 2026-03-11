namespace WebApplication4.Infrastructure.Reports
{
    public static class ReportQueries
    {
        public const string GroupLeadersAndLaggards = @"
        WITH StudentScores AS (
            SELECT s.id AS studentid,
                   u.firstname || ' ' || u.lastname AS fullname,
                   g.id AS groupid, 
                   g.name AS groupname,
                   d.name AS direction, 
                   c.name AS course,
                   COALESCE(SUM(a.score), 0) AS totalscore
            FROM students s
            JOIN users u ON s.userid = u.id
            JOIN student_groups sg ON s.id = sg.studentsid
            JOIN groups g ON sg.groupsid = g.id
            JOIN directions d ON g.directionid = d.id
            JOIN courses c ON g.courseid = c.id
            LEFT JOIN attempts a ON s.id = a.studentid
            WHERE (@DirectionId IS NULL OR g.directionid = @DirectionId)
              AND (@CourseId IS NULL OR g.courseid = @CourseId)
            GROUP BY s.id, u.firstname, u.lastname, g.id, g.name, d.name, c.name
        ),
        GroupMaxMin AS (
            -- Находим максимальный и минимальный балл в каждой группе
            SELECT groupid,
                   MAX(totalscore) AS max_score,
                   MIN(totalscore) AS min_score
            FROM StudentScores
            GROUP BY groupid
        )
        -- Join'им обратно, чтобы получить имена лидеров и отстающих
        SELECT ss.groupid, 
               ss.groupname, 
               ss.direction, 
               ss.course,
               MAX(CASE WHEN ss.totalscore = gmm.max_score THEN ss.fullname END) AS leadername,
               MAX(CASE WHEN ss.totalscore = gmm.max_score THEN ss.totalscore END) AS leaderscore,
               MAX(CASE WHEN ss.totalscore = gmm.min_score THEN ss.fullname END) AS laggardname,
               MAX(CASE WHEN ss.totalscore = gmm.min_score THEN ss.totalscore END) AS laggardscore
        FROM StudentScores ss
        JOIN GroupMaxMin gmm ON ss.groupid = gmm.groupid
        GROUP BY ss.groupid, ss.groupname, ss.direction, ss.course
        ORDER BY ss.groupname;";

        public const string StudentTestResults = @"
        WITH BestAttempt AS (
            SELECT DISTINCT ON (tr.testid)
                tr.testid, tr.attemptid, a.score, tr.passed, a.submittedat
            FROM testresults tr
            JOIN attempts a ON tr.attemptid = a.id
            WHERE tr.studentid = @StudentId
            ORDER BY tr.testid, a.score DESC NULLS LAST
        ),
        TestMaxScore AS (
            SELECT testid, COALESCE(SUM(maxscore), 10) AS maxscore
            FROM questions GROUP BY testid
        )
        SELECT t.id AS testid, t.title AS testtitle,
               COALESCE(ba.score, 0) AS bestscore,
               COALESCE(tms.maxscore, 10) AS maxpossiblescore,
               COALESCE(ba.passed, FALSE) AS passed,
               ba.submittedat AS completedat,
               COUNT(a.id) AS attemptscount
        FROM tests t
        JOIN test_groups tg ON t.id = tg.testsid
        JOIN student_groups sg ON tg.groupsid = sg.groupsid
        JOIN students s ON sg.studentsid = s.id
        LEFT JOIN BestAttempt ba ON ba.testid = t.id
        LEFT JOIN TestMaxScore tms ON tms.testid = t.id
        LEFT JOIN attempts a ON a.studentid = s.id AND a.testid = t.id
        WHERE s.id = @StudentId
        GROUP BY t.id, t.title, ba.score, ba.passed, ba.submittedat, tms.maxscore
        ORDER BY ba.submittedat DESC NULLS LAST, t.title;";
    }
}

