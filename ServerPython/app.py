#!flask/bin/python
# -*- coding: utf-8 -*-
from flask import Flask,jsonify,abort
from flask_httpauth import HTTPBasicAuth
from OpenSSL import SSL




auth=HTTPBasicAuth()
app=Flask(__name__)

@auth.get_password
def get_password(username):
 if username=='123':
  return '123'
 return None

@auth.error_handler
def unauthorized():
 return "Хватит Вводить всякую чушь"

def get_connect():
 return pymysql.connect(host='89.223.31.42', user='root', passwd='123', db='TestBase', charset="utf8")

def get_cur_in_sql(query):
    sql = query
    db = get_connect()
    cur = db.cursor()
    cur.execute(sql)
    return cur

def get_in_base_city():
 cur=get_cur_in_sql( "SELECT * FROM offices")
 return [dict(id=row[0],city=row[1],text=row[2]) for row in cur.fetchall()]

def get_in_base_user_dates():
 cur = get_cur_in_sql("select * from users as u left join savings as s on u.id_user=s.id_user")
 return [dict(id=row[0],city=row[1],text=row[2]) for row in cur.fetchall()]

def get_in_base(query_id):
 cur = get_cur_in_sql("SELECT * FROM Users WHERE id="+str(query_id))
 return cur.fetchone()

@app.route('/todo/api/v1.0/tasks',methods=['GET'])
def get_tasks():
 return jsonify({'tasks':tasks})

@app.route('/todo/api/v1.0/query/<int:query_id>',methods=['GET'])
@auth.login_required
def get_query(query_id):
 return str(get_in_base(query_id)[1].encode("utf8"))+" "+str(get_in_base(query_id)[2].encode("utf8"))+" "+str(get_in_base(query_id)[3].encode("utf8"))

@app.route('/todo/api/v1.0/query/get_city',methods=['GET'])
#@auth.login_required
def get_query_city():
 return jsonify({'cities':get_in_base_city()})




@app.route('/todo/api/v1.0/tasks/<int:task_id>',methods=['GET'])
@auth.login_required
def get_task_id(task_id):
 task=filter(lambda t:t['id']==task_id,tasks)
 if len(task)==0:
   return u"Пошел нахер рукожоп!!!"
 return jsonify({'task':task[0]})

@app.errorhandler(404)
def not_fount(error):
 return "Хватит уже вводить всякую дичь!!!!"

if(__name__=='__main__'):
 app.debug=True
 app.run(host='0.0.0.0')
