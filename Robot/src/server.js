const express = require('express');
const cors = require('cors');
const routes = require('./routes');
const app = express();

const port = '61203'

app.use(express.json());
app.use(cors());
app.use(routes);


app.listen(port, () => console.log('listening ', port));
