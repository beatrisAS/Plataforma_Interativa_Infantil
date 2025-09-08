from flask_sqlalchemy import SQLAlchemy
from datetime import datetime
import json

db = SQLAlchemy()

class Activity(db.Model):
    __tablename__ = 'activities'
    
    id = db.Column(db.Integer, primary_key=True)
    category = db.Column(db.String(50), nullable=False)
    question = db.Column(db.Text, nullable=False)
    question_type = db.Column(db.String(30), default='multiple_choice')
    answers = db.Column(db.Text, nullable=False)  # JSON string
    explanation = db.Column(db.Text)
    difficulty = db.Column(db.String(20), default='easy')
    points = db.Column(db.Integer, default=10)
    created_at = db.Column(db.DateTime, default=datetime.utcnow)
    is_active = db.Column(db.Boolean, default=True)
    
    def __init__(self, category, question, answers, explanation=None, difficulty='easy', points=10, question_type='multiple_choice'):
        self.category = category
        self.question = question
        self.answers = json.dumps(answers) if isinstance(answers, (list, dict)) else answers
        self.explanation = explanation
        self.difficulty = difficulty
        self.points = points
        self.question_type = question_type
    
    def to_dict(self):
        return {
            'id': self.id,
            'category': self.category,
            'question': self.question,
            'type': self.question_type,
            'answers': json.loads(self.answers) if self.answers else [],
            'explanation': self.explanation,
            'difficulty': self.difficulty,
            'points': self.points,
            'created_at': self.created_at.isoformat() if self.created_at else None
        }
    
    @staticmethod
    def get_by_category(category, limit=None):
        query = Activity.query.filter_by(category=category, is_active=True)
        if limit:
            query = query.limit(limit)
        return query.all()
    
    @staticmethod
    def get_random_by_category(category, limit=5):
        return Activity.query.filter_by(category=category, is_active=True).order_by(db.func.random()).limit(limit).all()

class UserProgress(db.Model):
    __tablename__ = 'user_progress'
    
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.String(100), nullable=False)  # Pode ser um ID de sessão ou usuário
    category = db.Column(db.String(50), nullable=False)
    activity_id = db.Column(db.Integer, db.ForeignKey('activities.id'), nullable=False)
    is_correct = db.Column(db.Boolean, nullable=False)
    points_earned = db.Column(db.Integer, default=0)
    time_taken = db.Column(db.Integer)  # em segundos
    completed_at = db.Column(db.DateTime, default=datetime.utcnow)
    
    activity = db.relationship('Activity', backref=db.backref('progress_records', lazy=True))
    
    def __init__(self, user_id, category, activity_id, is_correct, points_earned=0, time_taken=None):
        self.user_id = user_id
        self.category = category
        self.activity_id = activity_id
        self.is_correct = is_correct
        self.points_earned = points_earned
        self.time_taken = time_taken
    
    def to_dict(self):
        return {
            'id': self.id,
            'user_id': self.user_id,
            'category': self.category,
            'activity_id': self.activity_id,
            'is_correct': self.is_correct,
            'points_earned': self.points_earned,
            'time_taken': self.time_taken,
            'completed_at': self.completed_at.isoformat() if self.completed_at else None
        }
    
    @staticmethod
    def get_user_stats(user_id, category=None):
        query = UserProgress.query.filter_by(user_id=user_id)
        if category:
            query = query.filter_by(category=category)
        
        progress_records = query.all()
        
        total_questions = len(progress_records)
        correct_answers = sum(1 for record in progress_records if record.is_correct)
        total_points = sum(record.points_earned for record in progress_records)
        
        return {
            'total_questions': total_questions,
            'correct_answers': correct_answers,
            'total_points': total_points,
            'accuracy': (correct_answers / total_questions * 100) if total_questions > 0 else 0
        }

class Category(db.Model):
    __tablename__ = 'categories'
    
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(50), unique=True, nullable=False)
    display_name = db.Column(db.String(100), nullable=False)
    description = db.Column(db.Text)
    icon = db.Column(db.String(50))
    color = db.Column(db.String(20))
    is_active = db.Column(db.Boolean, default=True)
    order_index = db.Column(db.Integer, default=0)
    
    def __init__(self, name, display_name, description=None, icon=None, color=None, order_index=0):
        self.name = name
        self.display_name = display_name
        self.description = description
        self.icon = icon
        self.color = color
        self.order_index = order_index
    
    def to_dict(self):
        return {
            'id': self.id,
            'name': self.name,
            'display_name': self.display_name,
            'description': self.description,
            'icon': self.icon,
            'color': self.color,
            'is_active': self.is_active,
            'order_index': self.order_index
        }
    
    @staticmethod
    def get_active_categories():
        return Category.query.filter_by(is_active=True).order_by(Category.order_index).all()

