const express = require('express');
const sql = require('mssql');
const cors = require('cors');
const { type } = require('os');

const app = express();
app.use(cors());
app.use(express.json());

const config = {
  server: 'desktop-2580',
  options: {
    trustedservercertificate: true
  },
  authentication: {
    type: 'ntlm',
    options: {
      domain: 'DESKTOP-2580',
      userName: 'mattm',
      password: '270420M@tt!'
    }
  },
  database: 'data'
};

app.get('/api/words', (req, res) => {
  sql.connect(config).then(pool => {
    return pool.request().query('SELECT * FROM orders');
  }).then(result => {
    console.log('results:', result.recordset);
    res.json(result.recordset);
  }).catch(err => {
    console.error('error fetching words:', err);
    res.status(500).send('internal server error');
  });
});

app.listen(3000, () => {console.log('Server is running on port 3000');});
